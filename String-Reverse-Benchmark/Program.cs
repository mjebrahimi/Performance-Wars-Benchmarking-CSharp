using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNetVisualizer;
using System.Text;

var summary = BenchmarkAutoRunner.Run<Benchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "Benchmark of different methods to Reverse a string",
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = false,
    });

Console.ReadLine();

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class Benchmark
{
    const string text = "Although most people consider piranhas to be quite dangerous, they are, for the most part, entirely harmless. Piranhas rarely feed on large animals.they eat smaller fish and aquatic plants.";

    [Benchmark]
    public string Span_Reverse_ZeroAllocation()
    {
        Span<char> span = text.GetSpan();
        span.Reverse();
        return text;
    }

    [Benchmark]
    public string Span_Reverse()
    {
        Span<char> span = text.ToCharArray(); //OR (no difference)
        //Span<char> span = text.AsSpan().ToArray();
        span.Reverse();
        return new string(span); //span.ToString(); //the same
    }

    [Benchmark]
    public string Array_Reverse()
    {
        var charArray = text.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    [Benchmark]
    public string StringCreate()
    {
        return string.Create(text.Length, text, (chars, state) =>
        {
            var pos = 0;
            for (int i = state.Length - 1; i >= 0; i--)
                chars[pos++] = state[i];
        });
    }

    [Benchmark]
    public string StringBuilderCache()
    {
        var builder = System.Text.StringBuilderCache.Acquire(text.Length);
        for (int i = text.Length - 1; i >= 0; i--)
            builder.Append(text[i]);
        return System.Text.StringBuilderCache.GetStringAndRelease(builder);
    }

    [Benchmark]
    public string StringBuilder()
    {
        var builder = new StringBuilder(text.Length);
        for (int i = text.Length - 1; i >= 0; i--)
            builder.Append(text[i]);
        return builder.ToString();
    }

    [Benchmark]
    public string ValueStringBuilder()
    {
        using var builder = new ValueStringBuilder(text.Length);
        for (int i = text.Length - 1; i >= 0; i--)
            builder.Append(text[i]);
        return builder.ToString();
    }

    [Benchmark]
    public string StringWriter()
    {
        var writer = new StringWriter(); // uses StringBuilder under the hood
        for (int i = text.Length - 1; i >= 0; i--)
            writer.Write(text[i]);
        return writer.ToString();
    }

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
}

public static class StringExtensions
{
    public static unsafe Span<char> GetSpan(this string str)
    {
        //var handle = GCHandle.Alloc(str, GCHandleType.Pinned);
        //var address = handle.AddrOfPinnedObject();
        //return new Span<char>(address.ToPointer(), str.Length);

        fixed (char* ptr = str)
        {
            return new Span<char>(ptr, str.Length);
        }
    }
}