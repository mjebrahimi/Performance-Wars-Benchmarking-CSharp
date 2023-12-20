using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
#pragma warning disable S3903, CA1050, RCS1110
public class Benchmark2
#pragma warning restore S3903, CA1050, RCS1110
{
    private const int length = 1_000_000;
    private static readonly int[] arrayEmpty = [];
    private static readonly int[] arrayNotEmpty = [1, 2, 3];
    private static readonly List<int> listEmpty = [];
    private static readonly List<int> listNotEmpty = [1, 2, 3];

#pragma warning disable S1481 // Unused local variables should be removed
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1707 // Identifiers should not contains underscore

    #region UsingTryGetNonEnumeratedCount
    [Benchmark(Description = "UsingTryGetNonEnumeratedCount"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_UsingTryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty.IsNullOrEmpty_UsingTryGetNonEnumeratedCount();
        }
    }

    [Benchmark(Description = "UsingTryGetNonEnumeratedCount"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_UsingTryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty.IsNullOrEmpty_UsingTryGetNonEnumeratedCount();
        }
    }

    [Benchmark(Description = "UsingTryGetNonEnumeratedCount"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_UsingTryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty.IsNullOrEmpty_UsingTryGetNonEnumeratedCount();
        }
    }

    [Benchmark(Description = "UsingTryGetNonEnumeratedCount"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_UsingTryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty.IsNullOrEmpty_UsingTryGetNonEnumeratedCount();
        }
    }
    #endregion

    #region UsingPatternMatching_ForArray
    [Benchmark(Description = "UsingPatternMatching_ForArray"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_UsingPatternMatching_ForArray()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty.IsNullOrEmpty_UsingPatternMatching_ForArray();
        }
    }

    [Benchmark(Description = "UsingPatternMatching_ForArray"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_UsingPatternMatching_ForArray()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty.IsNullOrEmpty_UsingPatternMatching_ForArray();
        }
    }

    [Benchmark(Description = "UsingPatternMatching_ForArray"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_UsingPatternMatching_ForArray()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty.IsNullOrEmpty_UsingPatternMatching_ForArray();
        }
    }

    [Benchmark(Description = "UsingPatternMatching_ForArray"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_UsingPatternMatching_ForArray()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty.IsNullOrEmpty_UsingPatternMatching_ForArray();
        }
    }
    #endregion
#pragma warning restore CA1707 // Identifiers should not contains underscore
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S1481 // Unused local variables should be removed
}

public static class Excensions
{
    public static bool IsNullOrEmpty_UsingTryGetNonEnumeratedCount(this IEnumerable<int> source)
    {
        return source is null
            || (source.TryGetNonEnumeratedCount(out var count) && count == 0)
            || source.Any() is false;
    }

    public static bool IsNullOrEmpty_UsingPatternMatching_ForArray(this IEnumerable<int> source)
    {
        return source is null
            || source is Array and { Length: 0 }
            || (source.TryGetNonEnumeratedCount(out var count) && count == 0)
            || source.Any() is false;
    }
}