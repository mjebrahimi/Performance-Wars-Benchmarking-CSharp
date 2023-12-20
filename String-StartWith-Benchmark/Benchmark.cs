using BenchmarkDotNet.Attributes;
using Utilities;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[KeepBenchmarkFiles(false)]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByParams)]
public class Benchmark
{
    [Arguments("http://www.google.com")]
    [Arguments("https://www.google.com")]
    [Benchmark]
    public bool String_StartsWith(string str)
    {
        return str.StartsWith("https://");
    }

    [Arguments("http://www.google.com")]
    [Arguments("https://www.google.com")]
    [Benchmark]
    public bool Span_StartsWith(string str)
    {
        return str.AsSpan().StartsWith("https://".AsSpan());
    }

    [Arguments("http://www.google.com")]
    [Arguments("https://www.google.com")]
    [Benchmark]
    public bool SpanExt_StartsWith(string str)
    {
        return str.StartsWithSpan("https://");
    }

    #region Other Methods
    //Regex.IsMatch(str, "^Hello");
    //String.Compare(str, "Hello", StringComparison.Ordinal) == 0
    #endregion
}