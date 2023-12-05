using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using System.Text;
//[DryJob]
//[ShortRunJob]
[SimpleJob(RunStrategy.Throughput)]
[MemoryDiagnoser]
[KeepBenchmarkFiles(false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class Benchmark
{
    private readonly string[] arr = new[]
    {
        "Although",
        " ", "most",
        " ", "people",
        " ", "consider",
        " ", "piranhas",
        " ", "to",
        " ", "be",
        " ", "quite",
        " ", "dangerous",
        " ", "they",
        " ", "are",
        " ", "for",
        " ", "the",
        " ", "most",
        " ", "part",
        " ", "entirely",
        " ", "harmless",
        " ", "Piranhas",
        " ", "rarely",
        " ", "feed",
        " ", "on",
        " ", "large",
        " ", "animals",
        " ", "they",
        " ", "eat",
        " ", "smaller",
        " ", "fish",
        " ", "and",
        " ", "aquatic",
        " ", "plants"
    };

    [Benchmark]
    public string StringBuilderCache_Append()
    {
        var builder = StringBuilderCache.Acquire();
        for (int i = 0; i < arr.Length; i++)
            builder.Append(arr[i]);
        return StringBuilderCache.GetStringAndRelease(builder);
    }

    [Benchmark]
    public string StringConcat()
    {
        return string.Concat(arr);
    }

    [Benchmark]
    public string ValueStringBuilder_Append()
    {
        using var builder = new ValueStringBuilder();
        for (int i = 0; i < arr.Length; i++)
            builder.Append(arr[i]);
        return builder.ToString();
    }

    [Benchmark]
    public string StringBuilder_Append()
    {
        var builder = new StringBuilder();
        for (int i = 0; i < arr.Length; i++)
            builder.Append(arr[i]);
        return builder.ToString();
    }

    [Benchmark]
    public string StringJoin_Empty()
    {
        return string.Join(string.Empty, arr);
    }

    [Benchmark]
    public string LinqAggregate_StringBuilder()
    {
        var sb = arr.Aggregate(new StringBuilder(), (sb, str) => sb.Append(str));
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilder_AppendJoin_Empty()
    {
        var stringBuilder = new StringBuilder();
        return stringBuilder.AppendJoin(string.Empty, arr).ToString();
    }

    [Benchmark]
    public string LinqAggregate_Plus()
    {
        return arr.Aggregate((prev, current) => prev + current);
    }
}