using BenchmarkDotNet.Attributes;
using System.Text;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
[ShortRunJob]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
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
    public IEnumerable<object[]> GetParams() => CreateParams();

    //Has BUG!
    //[Benchmark]
    //[ArgumentsSource(nameof(GetParams))]
    //[BenchmarkOrder(Priority = 1)]
    //public string UsingBinaryReader(Stream stream, int TextLength, int BytesLength)
    //{
    //    string str = StreamToStringExtensions.UsingBinaryReader(stream);
    //    stream.Position = 0;
    //    return str;
    //}

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string UsingStreamReader_DefaultBufferSize(Stream stream, int TextLength, string Language)
    {
        string str = stream.UsingStreamReader(defaultBufferSize: true, detectEncoding: true);
        stream.Position = 0;
        return str;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string UsingStreamReader_CustomBufferSize(Stream stream, int TextLength, string Language)
    {
        string str = stream.UsingStreamReader(defaultBufferSize: false, detectEncoding: true);
        stream.Position = 0;
        return str;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string ReadBytesConvertToUTF8_GetString(Stream stream, int TextLength, string Language)
    {
        var str = stream.ReadBytesConvertToUTF8_GetString();
        stream.Position = 0;
        return str;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string ReadBytesConvertToUTF8_Encoding(Stream stream, int TextLength, string Language)
    {
        var str = stream.ReadBytesConvertToUTF8_Encoding();
        stream.Position = 0;
        return str;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string ReadBytesConvertToUTF8_Decoder(Stream stream, int TextLength, string Language)
    {
        var str = stream.ReadBytesConvertToUTF8_Decoder();
        stream.Position = 0;
        return str;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string ReadBytesConvertToUTF8_Encoding_SpanTrim(Stream stream, int TextLength, string Language)
    {
        var str = stream.ReadBytesConvertToUTF8_Encoding_SpanTrim();
        stream.Position = 0;
        return str;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    [BenchmarkOrder(Priority = 1)]
    public string ReadBytesConvertToUTF8_Encoding_ArrayResize(Stream stream, int TextLength, string Language)
    {
        var str = stream.ReadBytesConvertToUTF8_Encoding_ArrayResize();
        stream.Position = 0;
        return str;
    }

    //Has BUG!
    //[Benchmark]
    //[ArgumentsSource(nameof(GetParams))]
    //[BenchmarkOrder(Priority = 1)]
    //public string ReadBytesConvertToUTF8_Encoding_ArrayResize_WithBuffer(Stream stream, int TextLength, string Language)
    //{
    //    var str = stream.ReadBytesConvertToUTF8_Encoding_ArrayResize_WithBuffer();
    //    stream.Position = 0;
    //    return str;
    //}

    //[Benchmark]
    //[ArgumentsSource(nameof(GetParams))]
    //[BenchmarkOrder(Priority = 1)]
    //public string ReadBytesConvertToUTF8_Encoding_ArrayResize_WithBufferArrayPool(Stream stream, int TextLength, string Language)
    //{
    //    var str = stream.ReadBytesConvertToUTF8_Encoding_ArrayResize_WithBufferArrayPool();
    //    stream.Position = 0;
    //    return str;
    //}
    #endregion

    #region Async
    //public IEnumerable<object[]> GetAsyncParams() => CreateStreams(async: true);

    //[Benchmark]
    //[ArgumentsSource(nameof(GetAsyncParams))]
    //[BenchmarkOrder(Priority = 2)]
    //public async Task<byte[]> ReadAllBytesAsync(Stream stream, string FileStreamOption, int FileSize)
    //{
    //    var result = await stream.ReadAllBytesAsync();
    //    stream.Position = 0;
    //    return result;
    //}

    //[Benchmark]
    //[ArgumentsSource(nameof(GetAsyncParams))]
    //[BenchmarkOrder(Priority = 2)]
    //public async Task<byte[]> UsingRecyclableMemoryStreamAsync(Stream stream, string FileStreamOption, int FileSize)
    //{
    //    var result = await stream.UsingRecyclableMemoryStreamAsync();
    //    stream.Position = 0;
    //    return result;
    //}

    //[Benchmark]
    //[ArgumentsSource(nameof(GetAsyncParams))]
    //[BenchmarkOrder(Priority = 2)]
    //public async Task<byte[]> UsingMemoryStreamAsync(Stream stream, string FileStreamOption, int FileSize)
    //{
    //    var result = await stream.UsingMemoryStreamAsync();
    //    stream.Position = 0;
    //    return result;
    //}
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
    private const string smallEN = "Lorem ipsum dolor sit amet, consectetur adipiscin.";
    private const string smallFA = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از..";
    private const string mediumEN = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu ege";
    private const string mediumFA = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تمام";
    private const string largeEN = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu egestas. Vivamus eget consectetur nulla. Etiam pharetra, mi tincidunt consequat suscipit, nisi ligula mollis leo, non vehicula ante velit faucibus nisl. Nunc sit amet sapien nec justo malesuada fringilla vel.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu egestas. Vivamus eget consectetur nulla. Etiam pharetra, mi tincidunt consequat suscipit, nisi ligula mollis leo, non vehicula ante velit faucibus nisl. Nunc sit amet sapien nec justo malesuada fringilla vel.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit amet aliquet purus consectetur. Curabitur semper lorem maximus porta pharetra. Pellentesque venenatis ut dolor eu egestas. Vivamus eget consectetur nulla. Etiam pharetra, mi tincidunt consequat suscipit, nisi ligula mollis leo, non vehicula ante velit faucibus nisl. Nunc sit amet sapien nec justo malesuada fringilla vel.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam sed nunc neque. Mauris ut urna ullamcorper, sagittis sem vel, faucibus ligula. Cras vulputate quis arcu vel vulputate. Nam viverra sodales nulla, quis porttitor felis viverra commodo. Nulla facilisi. Integer blandit dui convallis magna malesuada, a posuere enim tincidunt. Aliquam tempus sem vitae ligula ultrices, sit am";
    private const string largeFA = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تماملورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تماملورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تماملورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تماملورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است و برای شرایط فعلی تکنولوژی مورد نیاز و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد. کتابهای زیادی در شصت و سه درصد گذشته، حال و آینده شناخت فراوان جامعه و متخصصان را می طلبد تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی و فرهنگ پیشرو در زبان فارسی ایجاد کرد. در این صورت می توان امید داشت که تمام";

    private static readonly List<(byte[] TextBytes, int TextLength)> texts = [
        //(Encoding.UTF8.GetBytes(smallEN), smallEN.Length),     //50
        //(Encoding.UTF8.GetBytes(smallFA), smallFA.Length),     //50
        //(Encoding.UTF8.GetBytes(mediumEN), mediumEN.Length),   //500
        //(Encoding.UTF8.GetBytes(mediumFA), mediumFA.Length),   //500
        //(Encoding.UTF8.GetBytes(largeEN), largeEN.Length),     //2,500
        //(Encoding.UTF8.GetBytes(largeFA), largeFA.Length),     //2,500
        //(Encoding.UTF8.GetBytes(largeEN + largeEN + largeEN + largeEN), largeEN.Length * 4),     //10,000
        (Encoding.UTF8.GetBytes(largeFA + largeFA + largeFA + largeFA), largeFA.Length * 4),     //10,000
    ];

    private static readonly List<Stream> streams = [];
    private static IEnumerable<object[]> CreateParams()
    {
        foreach (var (TextBytes, TextLength) in texts)
        {
            var memoryStream = new MemoryStream(TextBytes);
            streams.Add(memoryStream);

            var language = TextLength == TextBytes.Length ? "EN" : "FA";

            yield return [memoryStream, TextLength, language];
        }
    }

    //private static IEnumerable<object[]> CreateStreams(bool async)
    //{
    //    foreach (var length in lengths)
    //    {
    //        var bytes = GetRandomByteArray(length);

    //        var filename = Path.Combine(Path.GetTempPath(), "benchmark_temp", Guid.NewGuid() + ".tmp");
    //        var size = length / 1024; //KB

    //        File.WriteAllBytes(filename, bytes);

    //        var sequentialFS = CreateSequentialReadFileStream(filename);
    //        yield return [sequentialFS, "Sequential", size];

    //        if (async)
    //        {
    //            var sequentialAsyncFS = CreateSequentialReadAsyncFileStream(filename);
    //            yield return [sequentialAsyncFS, "Sequential-Async", size];
    //        }
    //    }

    //    static FileStream CreateSequentialReadFileStream(string path)
    //    {
    //        // SequentialScan is a perf hint that requires extra sys-call on non-Windows OSes.
    //        FileOptions options = OperatingSystem.IsWindows() ? FileOptions.SequentialScan : FileOptions.None;
    //        var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, options);

    //        streams.Add(fileStream);

    //        return fileStream;
    //    }

    //    static FileStream CreateSequentialReadAsyncFileStream(string path)
    //    {
    //        // SequentialScan is a perf hint that requires extra sys-call on non-Windows OSes.
    //        var options = FileOptions.Asynchronous | (OperatingSystem.IsWindows() ? FileOptions.SequentialScan : FileOptions.None);
    //        var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, options);

    //        streams.Add(fileStream);

    //        return fileStream;
    //    }

    //    static byte[] GetRandomByteArray(uint length)
    //    {
    //        var buffer = new byte[length];
    //        Random.Shared.NextBytes(buffer.AsSpan());
    //        return buffer;
    //    }
    //}
    #endregion
}