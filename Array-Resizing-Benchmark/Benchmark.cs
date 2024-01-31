using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

#if RELEASE
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[Config(typeof(CustomConfig))]
[HideColumns("array", "length")]
[MemoryDiagnoser(displayGenColumns: false)]
[KeepBenchmarkFiles(false)]
public class Benchmark
{
    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Array_Resize(ref byte[] array, int length, string Kind)
    {
        //It just Resize the array, not creating a new array, But others create a new array
        Array.Resize(ref array, length);
        return array;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Array_Copy(ref byte[] array, int length, string Kind)
    {
        var newArray = new byte[length];
        Array.Copy(array, newArray, length);
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Buffer_BlockCopy(ref byte[] array, int length, string Kind)
    {
        var newArray = new byte[length];
        Buffer.BlockCopy(array, 0, newArray, 0, length);
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Span_Slice_ToArray(ref byte[] array, int length, string Kind)
    {
#pragma warning disable IDE0057 // Use range operator
        return ((ReadOnlySpan<byte>)array).Slice(0, length).ToArray();
#pragma warning restore IDE0057 // Use range operator
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Span_Slice_CopyTo(ref byte[] array, int length, string Kind)
    {
        var newArray = new byte[length];
#pragma warning disable IDE0057 // Use range operator
        ((ReadOnlySpan<byte>)array).Slice(0, length).CopyTo(newArray);
#pragma warning restore IDE0057 // Use range operator
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Span_CollectionExpression_ToArray(ref byte[] array, int length, string Kind)
    {
        return ((ReadOnlySpan<byte>)array)[..length].ToArray();
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Span_CollectionExpression_CopyTo(ref byte[] array, int length, string Kind)
    {
        var newArray = new byte[length];
        ((ReadOnlySpan<byte>)array)[..length].CopyTo(newArray);
        return newArray;
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] RuntimeHelpers_GetSubArray(ref byte[] array, int length, string Kind)
    {
        return RuntimeHelpers.GetSubArray(array, ..length);
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Array_CollectionExpression(ref byte[] array, int length, string Kind)
    {
        return array[..length];
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Array_CollectionExpression_ToArray(ref byte[] array, int length, string Kind)
    {
#pragma warning disable IDE0305 // Simplify collection initialization
        return array[..length].ToArray();
#pragma warning restore IDE0305 // Simplify collection initialization
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] ArraySegment_ToArray(ref byte[] array, int length, string Kind)
    {
        var segment = new ArraySegment<byte>(array, 0, length);
#pragma warning disable IDE0305 // Simplify collection initialization
        return segment.ToArray();
#pragma warning restore IDE0305 // Simplify collection initialization
    }

    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] ArraySegment_CollectionExpression(ref byte[] array, int length, string Kind)
    {
        var segment = new ArraySegment<byte>(array, 0, length);
        return [.. segment];
    }


    [Benchmark]
    [ArgumentsSource(nameof(GetParams))]
    public byte[] Linq_Take_ToArray(ref byte[] array, int length, string Kind)
    {
        return array.Take(length).ToArray();
    }

    //[Benchmark]
    //[ArgumentsSource(nameof(GetParams))]
    //public byte[] SpanTrim(ref byte[] array, int length)
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