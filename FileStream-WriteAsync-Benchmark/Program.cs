using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;

var tempDirectory = Path.Combine(Path.GetTempPath(), "benchmark_temp");
Directory.CreateDirectory(tempDirectory);

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig()); //For Debugging
BenchmarkRunner.Run<Benchmark>(new BenchmarkDotNet.Configs.DebugInProcessConfig());

#else

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
BenchmarkRunner.Run<Benchmark>();

#endif

//Cleanup temp files
Directory.Delete(tempDirectory, true);

Console.ReadLine();

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
[ShortRunJob]
//[SimpleJob(RunStrategy.ColdStart)]
#endif
[Config(typeof(CustomConfig))]
[HideColumns("bytes")]
[MemoryDiagnoser(displayGenColumns: false)]
[KeepBenchmarkFiles(false)]
public class Benchmark
{
    [Benchmark]
    [ArgumentsSource(nameof(GetParams2))]
    [BenchmarkOrder(Priority = 1)]
    public void File_WriteAllBytes(byte[] bytes, string FileOptions, int FileSize)
    {
        var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
        File.WriteAllBytes(filename, bytes);
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public void FileStream_Write(byte[] bytes, FileOptions FileOptions, int FileSize)
    {
        var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
        using var fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions);
        fileStream.Write(bytes);
        //fileStream.Flush();
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams2))]
    [BenchmarkOrder(Priority = 2)]
    public async Task File_WriteAllBytesAsync(byte[] bytes, string FileOptions, int FileSize)
    {
        var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
        await File.WriteAllBytesAsync(filename, bytes);
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 2)]
    public async Task FileStream_WriteAsync(byte[] bytes, FileOptions FileOptions, int FileSize)
    {
        var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
        using var fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions);
        await fileStream.WriteAsync(bytes);
        //await fileStream.FlushAsync();
    }

    #region Utils
    private static readonly IEnumerable<byte[]> dumpFiles = [
        GetRandomByteArray(81920),      //=> 80KB
        GetRandomByteArray(1048576),    //=> 1MB
        //GetRandomByteArray(5_242_880) //=> 5MB //Run with [SimpleJob(RunStrategy.ColdStart)]
    ];

    private static byte[] GetRandomByteArray(uint length)
    {
        var buffer = new byte[length];
        Random.Shared.NextBytes(buffer.AsSpan());
        return buffer;
    }

    private static readonly List<Stream> streams = [];
    public static IEnumerable<object[]> GetParams()
    {
        foreach (var dumpFile in dumpFiles)
        {
            var size = dumpFile.Length / 1024; //KB

            yield return [dumpFile, FileOptions.None, size];
            yield return [dumpFile, FileOptions.Asynchronous, size];
        }
    }

    public static IEnumerable<object[]> GetParams2()
    {
        foreach (var dumpFile in dumpFiles)
        {
            var size = dumpFile.Length / 1024; //KB

            yield return [dumpFile, "X", size];
        }
    }
    #endregion

    #region CreateDumpFile
    public static void CreateDumpFile(string path, long fileSize)
    {
        var maxArraySize = Array.MaxLength; //2_147_483_591
        if (fileSize <= maxArraySize)
        {
            var buffer = new byte[fileSize];
            File.WriteAllBytes(path, buffer);
        }
        else
        {
            //TODO
            var buffer = new byte[1048576]; //1MB
            long currentFileSize = 0;

            using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.None);
            while (currentFileSize < fileSize)
            {
                fileStream.Write(buffer);
                currentFileSize += buffer.Length;
            }
            fileStream.Flush();
        }
    }

    public static async Task CreateDumpFileAsync(string path, long fileSize, CancellationToken cancellationToken)
    {
        var maxArraySize = Array.MaxLength; //2_147_483_591
        if (fileSize <= maxArraySize)
        {
            var buffer = new byte[fileSize];
            await File.WriteAllBytesAsync(path, buffer, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            //TODO
            var buffer = new byte[1048576]; //1MB
            long currentFileSize = 0;

            using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);
            while (currentFileSize < fileSize)
            {
                await fileStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
                currentFileSize += buffer.Length;
            }
            await fileStream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
    }
    #endregion

}