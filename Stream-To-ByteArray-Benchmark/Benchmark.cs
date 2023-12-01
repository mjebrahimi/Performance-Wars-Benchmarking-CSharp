using BenchmarkDotNet.Attributes;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[Config(typeof(CustomConfig))]
[HideColumns("stream")]
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
    public byte[] ReadAllBytes(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = stream.ReadAllBytes();
        stream.Position = 0;
        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public byte[] UsingBinaryReader(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = stream.UsingBinaryReader();
        stream.Position = 0;
        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public byte[] UsingMemoryStream(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = stream.UsingMemoryStream();
        stream.Position = 0;
        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public byte[] UsingRecyclableMemoryStream(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = stream.UsingRecyclableMemoryStream();
        stream.Position = 0;
        return result;
    }
    #endregion

    #region Async
    public IEnumerable<object[]> GetAsyncParams() => CreateStreams(async: true);

    [Benchmark]
    [ArgumentsSource(nameof(GetAsyncParams))]
    [BenchmarkOrder(Priority = 2)]
    public async Task<byte[]> ReadAllBytesAsync(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = await stream.ReadAllBytesAsync();
        stream.Position = 0;
        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetAsyncParams))]
    [BenchmarkOrder(Priority = 2)]
    public async Task<byte[]> UsingRecyclableMemoryStreamAsync(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = await stream.UsingRecyclableMemoryStreamAsync();
        stream.Position = 0;
        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetAsyncParams))]
    [BenchmarkOrder(Priority = 2)]
    public async Task<byte[]> UsingMemoryStreamAsync(Stream stream, string FileStreamOption, int FileSize)
    {
        var result = await stream.UsingMemoryStreamAsync();
        stream.Position = 0;
        return result;
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

            var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
            var size = length / 1024; //KB

            File.WriteAllBytes(filename, bytes);

            var sequentialFS = CreateSequentialReadFileStream(filename);
            yield return [sequentialFS, "Sequential", size];

            if (async)
            {
                var sequentialAsyncFS = CreateSequentialReadAsyncFileStream(filename);
                yield return [sequentialAsyncFS, "Sequential-Async", size];
            }
        }

        static FileStream CreateSequentialReadFileStream(string path)
        {
            // SequentialScan is a perf hint that requires extra sys-call on non-Windows OSes.
            FileOptions options = OperatingSystem.IsWindows() ? FileOptions.SequentialScan : FileOptions.None;
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, options);

            streams.Add(fileStream);

            return fileStream;
        }

        static FileStream CreateSequentialReadAsyncFileStream(string path)
        {
            // SequentialScan is a perf hint that requires extra sys-call on non-Windows OSes.
            var options = FileOptions.Asynchronous | (OperatingSystem.IsWindows() ? FileOptions.SequentialScan : FileOptions.None);
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, options);

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