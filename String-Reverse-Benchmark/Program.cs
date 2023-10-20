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
    const string input = "Although most people consider piranhas to be quite dangerous, they are, for the most part, entirely harmless. Piranhas rarely feed on large animals.they eat smaller fish and aquatic plants.";

    [Benchmark]
    public void LinqReverse_NewString()
    {
        _ = new string(input.Reverse().ToArray());
    }

    [Benchmark]
    public void LinqReverse_StringJoin()
    {
        _ = string.Join("", input.Reverse().ToArray());
    }

    [Benchmark]
    public void ArrayReverse_NewString()
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        _ = new string(charArray);
    }

    [Benchmark]
    public void StringBuilder_Reverse()
    {
        var builder = new StringBuilder(input.Length);
        for (int i = input.Length - 1; i >= 0; i--)
            builder.Append(input[i]);
        _ = builder.ToString();
    }
}