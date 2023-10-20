using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System.Text;

#if DEBUG
System.Console.ForegroundColor = System.ConsoleColor.Yellow;
System.Console.WriteLine("*****To achieve accurate results, set project configuration to Release mode.*****");
return;
#endif
BenchmarkRunner.Run<Benchmark>();

Console.ReadLine();

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
    public string StringConcat()
    {
        return string.Concat(arr);
    }

    [Benchmark]
    public string StringJoin_Empty()
    {
        return string.Join(string.Empty, arr);
    }

    [Benchmark]
    public string StringBuilder_Append()
    {
        var sbs = new StringBuilder();
        for (int i = 0; i < arr.Length; i++)
            sbs.Append(arr[i]);
        return sbs.ToString();
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

    [Benchmark]
    public string LinqAggregate_StringBuilder()
    {
        var sb = arr.Aggregate(new StringBuilder(), (sb, str) => sb.Append(str));
        return sb.ToString();
    }
}