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
    public string Reverse_StringBuilder()
    {
        return Reverse_StringBuilder(text);
    }

    [Benchmark]
    public string Reverse_StringCreate()
    {
        return Reverse_StringCreate(text);
    }

    [Benchmark]
    public string Reverse_Linq()
    {
        return Reverse_Linq(text);
    }

    [Benchmark]
    public string Reverse_Regex()
    {
        return Reverse_Regex(text);
    }

    public static string Reverse_StringCreate(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return string.Create(input.Length, input, (chars, state) =>
        {
            int reverseLength = 0;
            var length = chars.Length;
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

    public static string Reverse_StringBuilder(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new StringBuilder(input.Length);
        var currentWord = new StringBuilder(input.Length);
        foreach (char ch in input)
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

    public static string Reverse_Linq(string input)
    {
        var items = SplitAndKeepDelimiter(input, new[] { ' ', ',', '.' });
        var reversed = items.Select(p => p.Length == 1 ? p : Reverse(p));
        return string.Concat(reversed);

    }

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

    private static readonly Regex wordRegex = new(@"[^ ,\.]+", RegexOptions.Compiled);
    public static string Reverse_Regex(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return wordRegex.Replace(input, m => Reverse(m.Value));
    }

    private static string Reverse(string text)
    {
        var charArray = text.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}