using BenchmarkDotNet.Attributes;
using BenchmarkDotNetVisualizer;
using System.Buffers;

var bench = new ArrayCopyBenchmark();
var result1 = bench.Array_CopyTo_Span_Slice_Start(ArrayCopyBenchmark.GetTarget().First());
var result2 = bench.Array_CopyTo_Span_Slice_StartLength(ArrayCopyBenchmark.GetTarget().First());
var result3 = bench.Array_CopyTo_Span_CollectionExpression_Start(ArrayCopyBenchmark.GetTarget().First());
var result4 = bench.Array_CopyTo_Span_CollectionExpression_StartLength(ArrayCopyBenchmark.GetTarget().First());
var result5 = bench.Span_CopyTo_Span_Slice_Start(ArrayCopyBenchmark.GetTarget().First());
var result6 = bench.Span_CopyTo_Span_Slice_StartLength(ArrayCopyBenchmark.GetTarget().First());
var result7 = bench.Span_CopyTo_Span_CollectionExpression_Start(ArrayCopyBenchmark.GetTarget().First());
var result8 = bench.Span_CopyTo_Span_CollectionExpression_StartLength(ArrayCopyBenchmark.GetTarget().First());
var result9 = bench.Array_CopyTo_Array(ArrayCopyBenchmark.GetTarget().First());
var result10 = bench.Array_Copy(ArrayCopyBenchmark.GetTarget().First());
var result11 = bench.Buffer_BlockCopy(ArrayCopyBenchmark.GetTarget().First());

result1.ShouldBeSame(result2);
result1.ShouldBeSame(result3);
result1.ShouldBeSame(result4);
result1.ShouldBeSame(result5);
result1.ShouldBeSame(result6);
result1.ShouldBeSame(result7);
result1.ShouldBeSame(result8);
result1.ShouldBeSame(result9);
result1.ShouldBeSame(result10);
result1.ShouldBeSame(result11);

var summary = BenchmarkAutoRunner.Run<ArrayCopyBenchmark>();

var path = DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png");
await summary.SaveAsImageAsync(path, new ReportImageOptions
{
    Title = "Different ways to Copy an Array",
    SortByColumns = ["Mean", "Allocated"],
    SpectrumColumns = ["Mean", "Allocated"],
    GroupByColumns = [],
    HighlightGroups = false,
    DividerMode = RenderTableDividerMode.EmptyDividerRow,
});

#if RELEASE
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)] //more accurate results
#endif
[HideColumns("target")]
[MemoryDiagnoser(displayGenColumns: true)]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class ArrayCopyBenchmark
{
    const int len256k_target = 1 << 18; // more than LOH (85k)
    const int len128k_source = 1 << 17; // more than LOH (85k)
    const int len64k_startIndex = 1 << 16;

    private static readonly byte[] source;
    static ArrayCopyBenchmark()
    {
        var bytes = new byte[len128k_source];
        var random = new Random(986532147); //seeded random
        random.NextBytes(bytes);
        source = bytes;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Array_CopyTo_Span_Slice_Start(byte[] target)
    {
#pragma warning disable IDE0057 // Use range operator
        var span = ((Span<byte>)target).Slice(len64k_startIndex);
#pragma warning restore IDE0057 // Use range operator
        source.CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Array_CopyTo_Span_Slice_StartLength(byte[] target)
    {
        var span = ((Span<byte>)target).Slice(len64k_startIndex, source.Length);
        source.CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Array_CopyTo_Span_CollectionExpression_Start(byte[] target)
    {
        var span = ((Span<byte>)target)[len64k_startIndex..];
        source.CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Array_CopyTo_Span_CollectionExpression_StartLength(byte[] target)
    {
        var span = ((Span<byte>)target)[len64k_startIndex..(len64k_startIndex + source.Length)];
        source.CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Span_CopyTo_Span_Slice_Start(byte[] target)
    {
#pragma warning disable IDE0057 // Use range operator
        var span = ((Span<byte>)target).Slice(len64k_startIndex);
#pragma warning restore IDE0057 // Use range operator
        ((ReadOnlySpan<byte>)source).CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Span_CopyTo_Span_Slice_StartLength(byte[] target)
    {
        var span = ((Span<byte>)target).Slice(len64k_startIndex, source.Length);
        ((ReadOnlySpan<byte>)source).CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Span_CopyTo_Span_CollectionExpression_Start(byte[] target)
    {
        var span = ((Span<byte>)target)[len64k_startIndex..];
        ((ReadOnlySpan<byte>)source).CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Span_CopyTo_Span_CollectionExpression_StartLength(byte[] target)
    {
        var span = ((Span<byte>)target)[len64k_startIndex..(len64k_startIndex + source.Length)];
        ((ReadOnlySpan<byte>)source).CopyTo(span);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Array_CopyTo_Array(byte[] target)
    {
        source.CopyTo(target, len64k_startIndex);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Array_Copy(byte[] target)
    {
        Array.Copy(source, 0, target, len64k_startIndex, source.Length);
        return target;
    }

    [Benchmark, ArgumentsSource(nameof(GetTarget))]
    public byte[] Buffer_BlockCopy(byte[] target)
    {
        Buffer.BlockCopy(source, 0, target, len64k_startIndex, source.Length);
        return target;
    }

    public static IEnumerable<byte[]> GetTarget()
    {
        var bytes = new byte[len256k_target];
        var random = new Random(314151617); //seeded random
        random.NextBytes(bytes);

        yield return bytes;
    }
}

public static class Ext
{
    public static void ShouldBeSame(this byte[] source, byte[] target)
    {
        if (source.SequenceEqual(target) is false)
            throw new Exception("Are NOT the same.");
    }
}