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
    const string text = "Although most people consider piranhas to be quite dangerous, they are, for the most part, entirely harmless. Piranhas rarely feed on large animals.they eat smaller fish and aquatic plants.";

    [Benchmark]
    public string LinqReverse_NewString()
    {
        return new string(text.Reverse().ToArray());
    }

    [Benchmark]
    public string LinqReverse_StringJoin()
    {
        return string.Join("", text.Reverse().ToArray());
    }

    [Benchmark]
    public string StringBuilder_Reverse()
    {
        var builder = new StringBuilder(text.Length);
        for (int i = text.Length - 1; i >= 0; i--)
            builder.Append(text[i]);
        return builder.ToString();
    }

    [Benchmark]
    public string StringWriter_Reverse()
    {
        var sb = new StringWriter(); // uses underlying stringbuilder 
        for (int i = text.Length - 1; i >= 0; i--)
            sb.Write(text[i]);
        return sb.ToString();
    }

    [Benchmark]
    public string ArrayReverse_NewString()
    {
        var charArray = text.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    [Benchmark]
    public string Span_Reverse()
    {
        Span<char> span = text.ToCharArray();
        span.Reverse();
        return new string(span); //span.ToString(); //the same
    }

    [Benchmark]
    public string StringCreate_Reverse()
    {
        return string.Create(text.Length, text, (chars, state) =>
        {
            var pos = 0;
            for (int i = state.Length - 1; i >= 0; i--)
                chars[pos++] = state[i];
        });
    }
}