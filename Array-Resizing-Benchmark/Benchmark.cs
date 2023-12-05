using BenchmarkDotNet.Attributes;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
[ShortRunJob]
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[Config(typeof(CustomConfig))]
[HideColumns("array", "newSize")]
[MemoryDiagnoser(displayGenColumns: false)]
[KeepBenchmarkFiles(false)]
public class Benchmark
{

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Array_Resize(ref byte[] array, int newSize, string Kind)
    {
        //It just Resize the array, not creating a new array, But others create a new array
        Array.Resize(ref array, newSize);
        return array;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Array_Copy(ref byte[] array, int newSize, string Kind)
    {
        var newArray = new byte[newSize];
        Array.Copy(array, newArray, newSize);
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Buffer_BlockCopy(ref byte[] array, int newSize, string Kind)
    {
        var newArray = new byte[newSize];
        Buffer.BlockCopy(array, 0, newArray, 0, newSize);
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Span_CopyTo(ref byte[] array, int newSize, string Kind)
    {
        var newArray = new byte[newSize];
        ((ReadOnlySpan<byte>)array)[..newSize].CopyTo(newArray);
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Span_CopyTo_ToArray(ref byte[] array, int newSize, string Kind)
    {
        Span<byte> newArray = new byte[newSize];
        ((ReadOnlySpan<byte>)array)[..newSize].CopyTo(newArray);
        return newArray.ToArray();
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Linq_Take_ToArray(ref byte[] array, int newSize, string Kind)
    {
        return array.Take(newSize).ToArray();

    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] ArraySegment_ToArray(ref byte[] array, int newSize, string Kind)
    {
        var segment = new ArraySegment<byte>(array, 0, newSize);
        return segment.ToArray();
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] ArraySegment_CollectionExpression(ref byte[] array, int newSize, string Kind)
    {
        var segment = new ArraySegment<byte>(array, 0, newSize);
        return [.. segment];
    }

    //[Benchmark]
    //[ArgumentsSource(nameof(GetParams))]
    //public byte[] SpanTrim(ref byte[] array, int newSize)
    //{
    //    var newArray = ((ReadOnlySpan<byte>)array).Trim((byte)0);
    //    return newArray.ToArray();
    //}

    #region Utils
    public IEnumerable<object[]> GetParams()
    {
        var array = bytes.Concat(increces).ToArray();
        yield return [array, 1000, "Decrease"]; //Decrease the size from 2000 to 1000
        //yield return [bytes, 2000, "Increase"]; //Increase the size from 1000 to 2000
    }

    private static readonly byte[] bytes = GetRandomBytes(1000);
    private static readonly byte[] increces = new byte[1000];

    private static byte[] GetRandomBytes(int length)
    {
        var array = new byte[length];
        Random.Shared.NextBytes(array);
        return array;
    }
    #endregion
}