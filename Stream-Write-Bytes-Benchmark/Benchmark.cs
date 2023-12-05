using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
[ShortRunJob]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.ColdStart)]
#endif
[Config(typeof(CustomConfig))]
[HideColumns("stream", "bytes")]
[MemoryDiagnoser(displayGenColumns: false)]
[KeepBenchmarkFiles(false)]
//[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)] //Don't use because of CustomConfig.Orderer
//[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)] //Don't use because of CustomConfig.Orderer
public class Benchmark
{
    #region Sync
    public IEnumerable<object[]> GetParams() => CreateStreams(async: false);

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public void WriteAllBytes(Stream stream, byte[] bytes, string FileStreamOption, int FileSize)
    {
        stream.WriteAllBytes(bytes);
        stream.Position = 0;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public void WriteAllBytesSpan(Stream stream, byte[] bytes, string FileStreamOption, int FileSize)
    {
        stream.WriteAllBytesSpan(bytes);
        stream.Position = 0;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public void UsingBinaryWriter(Stream stream, byte[] bytes, string FileStreamOption, int FileSize)
    {
        stream.UsingBinaryWriter(bytes);
        stream.Position = 0;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public void UsingBinaryWriterSpan(Stream stream, byte[] bytes, string FileStreamOption, int FileSize)
    {
        stream.UsingBinaryWriterSpan(bytes);
        stream.Position = 0;
    }
    #endregion

    #region Async
    public IEnumerable<object[]> GetAsyncParams() => CreateStreams(async: true);

    [Benchmark]
    [ArgumentsSource(nameof(GetAsyncParams))]
    [BenchmarkOrder(Priority = 2)]
    public async Task WriteAllBytesAsync(Stream stream, byte[] bytes, string FileStreamOption, int FileSize)
    {
        await stream.WriteAllBytesAsync(bytes);
        stream.Position = 0;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetAsyncParams))]
    [BenchmarkOrder(Priority = 2)]
    public async Task WriteAllBytesSpanAsync(Stream stream, byte[] bytes, string FileStreamOption, int FileSize)
    {
        await stream.WriteAllBytesSpanAsync(bytes);
        stream.Position = 0;
    }
    #endregion

    [GlobalCleanup]
    public void Cleanup()
    {
        foreach (var stream in streams)
        {
            stream.Close();
            stream.Dispose();
        }
    }

    #region Utils
    private static readonly IEnumerable<uint> lengths = [
        4096,       //=> 4KB
        16384,      //=> 16KB
        81920,      //=> 80KB
        1048576,    //=> 1MB
    ];

    private static readonly List<Stream> streams = [];
    private static IEnumerable<object[]> CreateStreams(bool async)
    {
        foreach (var length in lengths)
        {
            var bytes = GetRandomByteArray(length);

            var size = length / 1024; //KB

            var stream = CreateFileStream();
            yield return [stream, bytes, "None", size];

            if (async)
            {
                var streamAsync = CreateAsyncFileStream();
                yield return [streamAsync, bytes, "Async", size];
            }
        }

        static FileStream CreateFileStream()
        {
            var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
            var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.None);

            streams.Add(fileStream);

            return fileStream;
        }

        static FileStream CreateAsyncFileStream()
        {
            var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
            var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);

            streams.Add(fileStream);

            return fileStream;
        }

        static byte[] GetRandomByteArray(uint length)
        {
            var buffer = new byte[length];
            Random.Shared.NextBytes(buffer.AsSpan());
            return buffer;
        }
    }
    #endregion
}