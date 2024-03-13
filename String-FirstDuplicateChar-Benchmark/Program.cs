using BenchmarkDotNet.Attributes;
using BenchmarkDotNetVisualizer;
using System.Runtime.CompilerServices;

var benchmark = new FirstDuplicateCharBenchmark();
Console.WriteLine(benchmark.WithInlineArray(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.WithStackAllocSpan(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.WithArray(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.TwoNestedFor(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.WithStringContainsCheck(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.WithHashSetAndAdd(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.WithHashSetAndContains(FirstDuplicateCharBenchmark.GivenString));
Console.WriteLine(benchmark.WithHashSetAndLinqAny(FirstDuplicateCharBenchmark.GivenString));

var summary = BenchmarkAutoRunner.Run<FirstDuplicateCharBenchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "Find First Char Appears Twice - Benchmark",
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
public class FirstDuplicateCharBenchmark
{
    //Constrains: all character are the English lowercase.
    public const string GivenString = "abcdqwertyuiopsfghjklzxvnmcbaacz"; //the first character that is appears twice is 'c'

    [InlineArray(26)]
    public struct InlineArray26<T>
    {
#pragma warning disable IDE0044, IDE0051, RCS1213, S1144 // Remove unused private members
        private T _element;
#pragma warning restore IDE0044, IDE0051, RCS1213, S1144 // Remove unused private members
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithInlineArray(string input)
    {
        var arr = new InlineArray26<bool>();
        //var span = MemoryMarshal.CreateSpan(ref Unsafe.As<InlineArray26<bool>, bool>(ref Unsafe.AsRef(in arr)), 26);
        foreach (var letter in input)
        {
            var index = letter - 'a';

            if (arr[index] is true)
            {
                return letter;
            }

            arr[index] = true;
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithStackAllocSpan(string input)
    {
        Span<bool> span = stackalloc bool[26];
        foreach (var letter in input)
        {
            var index = letter - 'a';

            if (span[index] is true)
            {
                return letter;
            }

            span[index] = true;
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithArray(string input)
    {
        var arr = new bool[26];
        //Span<bool> arr = new bool[26];
        foreach (var letter in input)
        {
            var index = letter - 'a';

            if (arr[index] is true)
            {
                return letter;
            }

            arr[index] = true;
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? TwoNestedFor(string input)
    {
        for (int i = 1; i < input.Length; i++)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (input[i].Equals(input[j]))
                    return input[i];
            }
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithStringContainsCheck(string input)
    {
        var tmp = "";

        foreach (var letter in input)
        {
            if (tmp.Contains(letter))
            {
                return letter;
            }
            else
            {
                tmp += letter;
            }
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithDictionaryContainsKey(string input)
    {
        Dictionary<char, int> letterIndexes = new();

        for (int i = 0; i < input.Length; i++)
        {
            char letter = input[i];
            if (letterIndexes.ContainsKey(letter))
            {
                return letter;
            }
            else
            {
                letterIndexes[letter] = i;
            }
        }

        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithHashSetAndAdd(string input)
    {
        HashSet<char> lettersSet = new();
        foreach (var letter in input)
        {
            if (lettersSet.Add(letter) == false)
            {
                return letter;
            }
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithHashSetAndContains(string input)
    {
        HashSet<char> hashSet = [];
        foreach (var letter in input)
        {
            if (hashSet.Contains(letter))
            {
                return letter;
            }
            else
            {
                hashSet.Add(letter);
            }
        }
        return null;
    }

    [Benchmark, Arguments(GivenString)]
    public char? WithHashSetAndLinqAny(string input)
    {
        HashSet<char> hashSet = [];
        foreach (var letter in input)
        {
            if (hashSet.Any(r => r == letter))
            {
                return letter;
            }
            else
            {
                hashSet.Add(letter);
            }
        }
        return null;
    }
}