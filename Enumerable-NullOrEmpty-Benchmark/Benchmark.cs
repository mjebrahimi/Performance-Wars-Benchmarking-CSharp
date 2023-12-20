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
public class Benchmark
#pragma warning restore S3903, CA1050, RCS1110
{
    private const int length = 1_000_000;

#pragma warning disable S3459 // Unassigned members should be removed
#pragma warning disable CS0649 // Field 'Benchmark.arrayNull' is never assigned to, and will always have its default value null
    private static readonly int[] arrayNull;
    private static readonly List<int> listNull;
#pragma warning restore CS0649 // Field 'Benchmark.arrayNull' is never assigned to, and will always have its default value null
#pragma warning restore S3459 // Unassigned members should be removed

    private static readonly int[] arrayEmpty = [];
    private static readonly List<int> listEmpty = [];

    private static readonly int[] arrayNotEmpty = [1, 2, 3];
    private static readonly List<int> listNotEmpty = [1, 2, 3];

#pragma warning disable S1481 // Unused local variables should be removed
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1707 // Identifiers should not contains underscore

    #region IfNullOrLength
    [Benchmark(Description = "IfNullOrLength"), BenchmarkCategory("Array (Null)")]
    public void Array_Null_IfNullOrLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNull is null || arrayNull.Length == 0;
        }
    }

    [Benchmark(Description = "IfNullOrLength"), BenchmarkCategory("List (Null)")]
    public void List_Null_IfNullOrLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNull is null || listNull.Count == 0;
        }
    }

    [Benchmark(Description = "IfNullOrLength"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_IfNullOrLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty is null || arrayEmpty.Length == 0;
        }
    }

    [Benchmark(Description = "IfNullOrLength"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_IfNullOrLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty is null || listEmpty.Count == 0;
        }
    }

    [Benchmark(Description = "IfNullOrLength"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_IfNullOrLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty is null || arrayNotEmpty.Length == 0;
        }
    }

    [Benchmark(Description = "IfNullOrLength"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_IfNullOrLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty is null || listNotEmpty.Count == 0;
        }
    }
    #endregion

    #region ConditionalIfLength
    [Benchmark(Description = "ConditionalIfLength"), BenchmarkCategory("Array (Null)")]
    public void Array_Null_ConditionalIfLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = !(arrayNull?.Length > 0);
        }
    }

    [Benchmark(Description = "ConditionalIfLength"), BenchmarkCategory("List (Null)")]
    public void List_Null_ConditionalIfLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = !(listNull?.Count > 0);
        }
    }

    [Benchmark(Description = "ConditionalIfLength"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_ConditionalIfLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = !(arrayEmpty?.Length > 0);
        }
    }

    [Benchmark(Description = "ConditionalIfLength"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_ConditionalIfLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = !(listEmpty?.Count > 0);
        }
    }

    [Benchmark(Description = "ConditionalIfLength"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_ConditionalIfLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = !(arrayNotEmpty?.Length > 0);
        }
    }

    [Benchmark(Description = "ConditionalIfLength"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_ConditionalIfLength()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = !(listNotEmpty?.Count > 0);
        }
    }
    #endregion

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method

    #region IfNullOrAny
    [Benchmark(Description = "IfNullOrAny"), BenchmarkCategory("Array (Null)")]
    public void Array_Null_IfNullOrAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNull is null || !arrayNull.Any();
        }
    }

    [Benchmark(Description = "IfNullOrAny"), BenchmarkCategory("List (Null)")]
    public void List_Null_IfNullOrAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNull is null || !listNull.Any();
        }
    }

    [Benchmark(Description = "IfNullOrAny"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_IfNullOrAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty is null || !arrayEmpty.Any();
        }
    }

    [Benchmark(Description = "IfNullOrAny"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_IfNullOrAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty is null || !listEmpty.Any();
        }
    }

    [Benchmark(Description = "IfNullOrAny"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_IfNullOrAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty is null || !arrayNotEmpty.Any();
        }
    }

    [Benchmark(Description = "IfNullOrAny"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_IfNullOrAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty is null || !listNotEmpty.Any();
        }
    }
    #endregion

    #region ConditionalIfAny
    [Benchmark(Description = "ConditionalIfAny"), BenchmarkCategory("Array (Null)")]
    public void Array_Null_ConditionalIfAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNull?.Any() != true;
        }
    }

    [Benchmark(Description = "ConditionalIfAny"), BenchmarkCategory("List (Null)")]
    public void List_Null_ConditionalIfAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNull?.Any() != true;
        }
    }

    [Benchmark(Description = "ConditionalIfAny"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_ConditionalIfAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty?.Any() != true;
        }
    }

    [Benchmark(Description = "ConditionalIfAny"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_ConditionalIfAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty?.Any() != true;
        }
    }

    [Benchmark(Description = "ConditionalIfAny"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_ConditionalIfAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty?.Any() != true;
        }
    }

    [Benchmark(Description = "ConditionalIfAny"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_ConditionalIfAny()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty?.Any() != true;
        }
    }
    #endregion

#pragma warning restore CA1860 // Avoid using 'Enumerable.Any()' extension method

    #region PatternMatching
    [Benchmark(Description = "PatternMatching"), BenchmarkCategory("Array (Null)")]
    public void Array_Null_PatternMatching()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNull is not { Length: > 0 };
        }
    }

    [Benchmark(Description = "PatternMatching"), BenchmarkCategory("List (Null)")]
    public void List_Null_PatternMatching()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNull is not { Count: > 0 };
        }
    }

    [Benchmark(Description = "PatternMatching"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_PatternMatching()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty is not { Length: > 0 };
        }
    }

    [Benchmark(Description = "PatternMatching"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_PatternMatching()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty is not { Count: > 0 };
        }
    }

    [Benchmark(Description = "PatternMatching"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_PatternMatching()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty is not { Length: > 0 };
        }
    }

    [Benchmark(Description = "PatternMatching"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_PatternMatching()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty is not { Count: > 0 };
        }
    }
    #endregion

    #region TryGetNonEnumeratedCount
    [Benchmark(Description = "TryGetNonEnumeratedCount"), BenchmarkCategory("Array (Null)")]
    public void Array_Null_TryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNull is null || (arrayNull.TryGetNonEnumeratedCount(out var count) && count == 0);
        }
    }

    [Benchmark(Description = "TryGetNonEnumeratedCount"), BenchmarkCategory("List (Null)")]
    public void List_Null_TryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNull is null || (listNull.TryGetNonEnumeratedCount(out var count) && count == 0);
        }
    }

    [Benchmark(Description = "TryGetNonEnumeratedCount"), BenchmarkCategory("Array (Empty)")]
    public void Array_Empty_TryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayEmpty is null || (arrayEmpty.TryGetNonEnumeratedCount(out var count) && count == 0);
        }
    }

    [Benchmark(Description = "TryGetNonEnumeratedCount"), BenchmarkCategory("List (Empty)")]
    public void List_Empty_TryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listEmpty is null || (listEmpty.TryGetNonEnumeratedCount(out var count) && count == 0);
        }
    }

    [Benchmark(Description = "TryGetNonEnumeratedCount"), BenchmarkCategory("Array (NoEmpty)")]
    public void Array_NotEmpty_TryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = arrayNotEmpty is null || (arrayNotEmpty.TryGetNonEnumeratedCount(out var count) && count == 0);
        }
    }

    [Benchmark(Description = "TryGetNonEnumeratedCount"), BenchmarkCategory("List (NoEmpty)")]
    public void List_NotEmpty_TryGetNonEnumeratedCount()
    {
        for (int i = 0; i < length; i++)
        {
            var _ = listNotEmpty is null || (listNotEmpty.TryGetNonEnumeratedCount(out var count) && count == 0);
        }
    }
    #endregion

#pragma warning restore CA1707 // Identifiers should not contains underscore
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore S1481 // Unused local variables should be removed
}