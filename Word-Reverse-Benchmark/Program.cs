using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System.Text;
using System.Text.RegularExpressions;

#if DEBUG
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("*****To achieve accurate results, set project configuration to Release mode.*****");
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
    public string StringCreate()
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var length = text.Length;
        return string.Create(length, text, (chars, state) =>
        {
            int reverseLength = 0;
            for (int i = 0; i < length; i++)
            {
                var ch = state[i];
                if (ch == ' ' || ch == ',' || ch == '.')
                {
                    if (reverseLength > 0)
                        chars.Slice(i - reverseLength, reverseLength).Reverse();
                    reverseLength = 0;
                }
                else
                {
                    reverseLength++;
                }
                chars[i] = ch;
            }
            if (reverseLength > 0)
            {
                chars.Slice(length - reverseLength, reverseLength).Reverse();
            }
        });
    }

    [Benchmark]
    public string StringBuilder()
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var length = text.Length;
        var result = new StringBuilder(length);
        var currentWord = new StringBuilder(length);
        foreach (char ch in text)
        {
            if (ch == ' ' || ch == ',' || ch == '.')
            {
                if (currentWord.Length > 0)
                {
                    result.Append(Reverse(currentWord.ToString()));
                    currentWord.Clear();
                }
                result.Append(ch);
            }
            else
            {
                currentWord.Append(ch);
            }
        }

        if (currentWord.Length > 0)
            result.Append(Reverse(currentWord.ToString()));

        return result.ToString();
    }

    [Benchmark]
    public string StringBuilderCache()
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var length = text.Length;
        var result = System.Text.StringBuilderCache.Acquire(length);
        var currentWord = System.Text.StringBuilderCache.Acquire(length);
        foreach (char ch in text)
        {
            if (ch == ' ' || ch == ',' || ch == '.')
            {
                if (currentWord.Length > 0)
                {
                    var str = System.Text.StringBuilderCache.GetStringAndRelease(currentWord);
                    result.Append(Reverse(str));
                    currentWord.Clear();
                }
                result.Append(ch);
            }
            else
            {
                currentWord.Append(ch);
            }
        }

        if (currentWord.Length > 0)
        {
            var str = System.Text.StringBuilderCache.GetStringAndRelease(currentWord);
            result.Append(Reverse(str));
        }

        return System.Text.StringBuilderCache.GetStringAndRelease(result);
    }

    [Benchmark]
    public string ValueStringBuilder()
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var length = text.Length;
        var result = new ValueStringBuilder(length);
        var currentWord = new ValueStringBuilder(length);
        foreach (char ch in text)
        {
            if (ch == ' ' || ch == ',' || ch == '.')
            {
                if (currentWord.Length > 0)
                {
                    result.Append(Reverse(currentWord.ToString()));
                    currentWord.Clear();
                }
                result.Append(ch);
            }
            else
            {
                currentWord.Append(ch);
            }
        }

        if (currentWord.Length > 0)
            result.Append(Reverse(currentWord.ToString()));

        return result.ToString();
    }

    [Benchmark]
    public string Linq()
    {
        var items = SplitAndKeepDelimiter(text, new[] { ' ', ',', '.' });
        var reversed = items.Select(p => p.Length == 1 ? p : Reverse(p));
        return string.Concat(reversed);
    }

    [Benchmark]
    public string Regex()
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return wordRegex.Replace(text, m => Reverse(m.Value));
    }

    private static readonly Regex wordRegex = new(@"[^ ,\.]+", RegexOptions.Compiled);
    private static IEnumerable<string> SplitAndKeepDelimiter(string str, char[] delims)
    {
        int start = 0, index;

        while ((index = str.IndexOfAny(delims, start)) != -1)
        {
            if (index - start > 0)
                yield return str.Substring(start, index - start);
            yield return str.Substring(index, 1);
            start = index + 1;
        }

        if (start < str.Length)
            yield return str.Substring(start);
    }
    private static string Reverse(string input)
    {
        var charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}