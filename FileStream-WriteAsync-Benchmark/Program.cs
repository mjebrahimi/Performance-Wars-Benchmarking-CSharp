using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

#if DEBUG
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("*****To achieve accurate results, set project configuration to Release mode.*****");
return;
#endif
BenchmarkRunner.Run<Benchmark>();

Console.ReadLine();

//[DryJob]
[ShortRunJob]
//[SimpleJob(RunStrategy.Throughput)]
[MemoryDiagnoser]
[KeepBenchmarkFiles(false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Benchmark
{
    const int fileSize = 500_000_000;

    [GlobalCleanup]
    public void Cleanup()
    {
        File.Delete("D:\\data1.dump");
        File.Delete("D:\\data2.dump");
        File.Delete("D:\\data3.dump");
        File.Delete("D:\\data4.dump");
        File.Delete("D:\\data5.dump");
        File.Delete("D:\\data6.dump");
        File.Delete("D:\\data7.dump");
        File.Delete("D:\\data8.dump");
        File.Delete("D:\\data9.dump");
        File.Delete("D:\\data10.dump");
    }

    [Benchmark]
    public void CreateDumpFile()
    {
        using var fileStream = new FileStream("D:\\data1.dump", FileMode.Create, FileAccess.Write);
        CreateDumpFile(fileStream, fileSize);
    }

    [Benchmark]
    public async Task CreateDumpFile_Async()
    {
        using var fileStream = new FileStream("D:\\data2.dump", FileMode.Create, FileAccess.Write);
        await CreateDumpFileAsync(fileStream, fileSize);
    }

    [Benchmark]
    public void CreateDumpFile_DeleteOnClose()
    {
        using var fileStream = new FileStream("D:\\data3.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.DeleteOnClose);
        CreateDumpFile(fileStream, fileSize);
    }

    [Benchmark]
    public async Task CreateDumpFile_DeleteOnClose_Async()
    {
        using var fileStream = new FileStream("D:\\data4.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.DeleteOnClose);
        await CreateDumpFileAsync(fileStream, fileSize);
    }

    [Benchmark]
    public void CreateDumpFile_SequentialScan()
    {
        using var fileStream = new FileStream("D:\\data5.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan);
        CreateDumpFile(fileStream, fileSize);
    }

    [Benchmark]
    public async Task CreateDumpFile_SequentialScan_Async()
    {
        using var fileStream = new FileStream("D:\\data6.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan);
        await CreateDumpFileAsync(fileStream, fileSize);
    }

    [Benchmark]
    public void CreateDumpFile_RandomAccess()
    {
        using var fileStream = new FileStream("D:\\data7.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.RandomAccess);
        CreateDumpFile(fileStream, fileSize);
    }

    [Benchmark]
    public async Task CreateDumpFile_RandomAccess_Async()
    {
        using var fileStream = new FileStream("D:\\data8.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.RandomAccess);
        await CreateDumpFileAsync(fileStream, fileSize);
    }

    [Benchmark]
    public void CreateDumpFile_WriteThrough()
    {
        using var fileStream = new FileStream("D:\\data9.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);
        CreateDumpFile(fileStream, fileSize);
    }

    [Benchmark]
    public async Task CreateDumpFile_WriteThrough_Async()
    {
        using var fileStream = new FileStream("D:\\data10.dump", FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);
        await CreateDumpFileAsync(fileStream, fileSize);
    }

    #region CreateDumpFile
    public static void CreateDumpFile(FileStream fileStream, long fileSize)
    {
        const int maxArraySize = 2_147_483_591;
        if (fileSize <= maxArraySize)
        {
            byte[] buffer = new byte[fileSize];
            fileStream.WriteAsync(buffer, 0, buffer.Length);
        }
        else
        {
            //TODO
            long currentFileSize = 0;
            while (currentFileSize < fileSize)
            {
                byte[] buffer = new byte[1024];
                fileStream.WriteAsync(buffer, 0, buffer.Length);
                currentFileSize += buffer.Length;
            }
        }
        fileStream.FlushAsync();
    }

    public static async Task CreateDumpFileAsync(FileStream fileStream, long fileSize)
    {
        const int maxArraySize = 2_147_483_591;
        if (fileSize <= maxArraySize)
        {
            byte[] buffer = new byte[fileSize];
            await fileStream.WriteAsync(buffer).ConfigureAwait(false);
        }
        else
        {
            //TODO
            long currentFileSize = 0;
            while (currentFileSize < fileSize)
            {
                byte[] buffer = new byte[1024];
                await fileStream.WriteAsync(buffer).ConfigureAwait(false);
                currentFileSize += buffer.Length;
            }
        }
        await fileStream.FlushAsync().ConfigureAwait(false);
    }
    #endregion
}