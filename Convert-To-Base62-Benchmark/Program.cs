using BenchmarkDotNet.Attributes;
using BenchmarkDotNetVisualizer;
using System.Text;

var bench = new ConvertToBase62Benchmark();
var expected = "2akka1";
bench.Using_SpanStackAlloc_CalculatedSize(int.MaxValue).ShouldBe(expected);
bench.Using_SpanStackAlloc_FixedSize(int.MaxValue).ShouldBe(expected);
bench.Using_SpanStackAlloc_FixedSize_StringToSpan(int.MaxValue).ShouldBe(expected);
bench.Using_SpanStackAlloc_FixedSize_CharArrayToSpan(int.MaxValue).ShouldBe(expected);
bench.Using_SpanStackAlloc_FixedSize_CharArray(int.MaxValue).ShouldBe(expected);
bench.Using_Array(int.MaxValue).ShouldBe(expected);
bench.Using_Linq(int.MaxValue).ShouldBe(expected);
bench.Using_StringCreate(int.MaxValue).ShouldBe(expected);
bench.Using_StringBuilder_FixedSize(int.MaxValue).ShouldBe(expected);
bench.Using_StringBuilder_doWhile(int.MaxValue).ShouldBe(expected);
bench.Using_StringBuilder_for(int.MaxValue).ShouldBe(expected);
bench.Using_StringBuilderCache(int.MaxValue).ShouldBe(expected);
bench.Using_StringBuilderCache_FixedSize(int.MaxValue).ShouldBe(expected);
bench.Using_ValueStringBuilder(int.MaxValue).ShouldBe(expected);
bench.Using_ValueStringBuilder_FixedSize(int.MaxValue).ShouldBe(expected);

var summary = BenchmarkAutoRunner.Run<ConvertToBase62Benchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "Convert A Number to Base62 String - Benchmark",
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = false,
    });

Console.ReadLine();

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)] //more accurate results
#endif
[HideColumns("input")]
[MemoryDiagnoser(false)]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
public class ConvertToBase62Benchmark
{
    private const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-";
    private static readonly char[] charsArray = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-".ToCharArray();

    #region SpanStackAlloc
    [Benchmark, Arguments(long.MaxValue)]
    public string Using_SpanStackAlloc_CalculatedSize(long input)
    {
        var charLength = chars.Length;
        var length = (int)Math.Ceiling(Math.Log(input, charLength));
        Span<char> span = stackalloc char[length];

        var index = length;
        do
        {
            index--;
            var charIndex = (int)(input % charLength);
            span[index] = chars[charIndex];
            input /= chars.Length;
        } while (input > 0);

        return new string(span);
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_SpanStackAlloc_FixedSize(long input)
    {
        var charLength = chars.Length;
        Span<char> span = stackalloc char[11];

        var index = 11;
        do
        {
            index--;
            var charIndex = (int)(input % charLength);
            span[index] = chars[charIndex];
            input /= charLength;
        } while (input > 0);

        return new string(span.Slice(index));
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_SpanStackAlloc_FixedSize_CharArray(long input)
    {
        var charArray = charsArray;
        var charLength = charArray.Length;
        Span<char> span = stackalloc char[11];

        var index = 11;
        do
        {
            index--;
            var charIndex = (int)(input % charLength);
            span[index] = charArray[charIndex];
            input /= charLength;
        } while (input > 0);

        return new string(span.Slice(index));
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_SpanStackAlloc_FixedSize_CharArrayToSpan(long input)
    {
        var charsSpan = charsArray.AsSpan();
        var charLength = charsSpan.Length;
        Span<char> span = stackalloc char[11];

        var index = 11;
        do
        {
            index--;
            var charIndex = (int)(input % charLength);
            span[index] = charsSpan[charIndex];
            input /= charLength;
        } while (input > 0);

        return new string(span.Slice(index));
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_SpanStackAlloc_FixedSize_StringToSpan(long input)
    {
        var charsSpan = chars.AsSpan();
        var charLength = charsSpan.Length;
        Span<char> span = stackalloc char[11];

        var index = 11;
        do
        {
            index--;
            var charIndex = (int)(input % charLength);
            span[index] = charsSpan[charIndex];
            input /= charLength;
        } while (input > 0);

        return new string(span.Slice(index));
    }
    #endregion

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_Array(long input)
    {
        var array = new char[11];
        var index = 11;
        do
        {
            index--;
            var charIndex = (int)(input % 63);
            array[index] = chars[charIndex];
            input /= 63;
        } while (input > 0);

        return new string(array, index, 11 - index);
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_Linq(long input)
    {
        var index = 11;
        var charArray = Enumerable.Range(0, 11)
                                  .Select(_ =>
                                  {
                                      if (input == 0)
                                          return default;
                                      else
                                          index--;

                                      var charIndex = (int)(input % 63);
                                      var selectedChar = chars[charIndex];
                                      input /= 63;
                                      return selectedChar;
                                  })
                                  .Reverse()
                                  .ToArray();

        return new string(charArray, index, 11 - index);
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_StringCreate(long input)
    {
        var length = (int)Math.Ceiling(Math.Log(input, 63));

        return string.Create(length, input, (span, state) =>
        {
            for (int i = length - 1; i >= 0; i--)
            {
                span[i] = chars[(int)(state % 63)];
                state /= 63;
            }
        });
    }

    #region StringBuilder
    [Benchmark, Arguments(long.MaxValue)]
    public string Using_StringBuilder_FixedSize(long input)
    {
        var builder = new StringBuilder(11);

        do
        {
            var charIndex = (int)(input % 63);
            var selectedChar = chars[charIndex];
            builder.Insert(0, selectedChar);
            input /= 63;
        } while (input > 0);

        return builder.ToString();
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_StringBuilder_doWhile(long input)
    {
        var builder = new StringBuilder();

        do
        {
            var charIndex = (int)(input % 63);
            var selectedChar = chars[charIndex];
            builder.Insert(0, selectedChar);
            input /= 63;
        } while (input > 0);

        return builder.ToString();
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_StringBuilder_for(long input)
    {
        var builder = new StringBuilder();

        for (; input > 0; input /= 63)
        {
            var charIndex = (int)(input % 63);
            var selectedChar = chars[charIndex];
            builder.Insert(0, selectedChar);
        }

        return builder.ToString();
    }
    #endregion

    #region StringBuilderCache
    [Benchmark, Arguments(long.MaxValue)]
    public string Using_StringBuilderCache(long input)
    {
        var builder = StringBuilderCache.Acquire();

        do
        {
            builder.Insert(0, chars[(int)(input % 63)]);
            input /= 63;
        } while (input > 0);

        return StringBuilderCache.GetStringAndRelease(builder);
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_StringBuilderCache_FixedSize(long input)
    {
        var builder = StringBuilderCache.Acquire(11);

        do
        {
            builder.Insert(0, chars[(int)(input % 63)]);
            input /= 63;
        } while (input > 0);

        return StringBuilderCache.GetStringAndRelease(builder);
    }
    #endregion

    #region ValueStringBuilder
    [Benchmark, Arguments(long.MaxValue)]
    public string Using_ValueStringBuilder(long input)
    {
        using var builder = new ValueStringBuilder();

        do
        {
            builder.Insert(0, chars[(int)(input % 63)], 1);
            input /= 63;
        } while (input > 0);

        return builder.ToString();
    }

    [Benchmark, Arguments(long.MaxValue)]
    public string Using_ValueStringBuilder_FixedSize(long input)
    {
        using var builder = new ValueStringBuilder(11);

        do
        {
            builder.Insert(0, chars[(int)(input % 63)], 1);
            input /= 63;
        } while (input > 0);

        return builder.ToString();
    }
    #endregion
}

public static class VerifyExt
{
    public static void ShouldBe(this string str, string expected)
    {
        if (str != expected)
            throw new ArgumentException(expected);
    }
}