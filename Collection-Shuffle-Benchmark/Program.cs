using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

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
    private IEnumerable<int> rangeEnum;
    private List<int> rangeList;
    private int[] rangeArray;

    [GlobalSetup]
    public void Setup()
    {
        rangeEnum = Enumerable.Range(1, 1_000);
        rangeList = rangeEnum.ToList();
        rangeArray = rangeList.ToArray();
    }

    #region Shuffle1_RandomShared
    [Benchmark]
    public void Shuffle1_Enumerable_RandomShared()
    {
        var _ = Shuffle1(rangeArray, Random.Shared).ToArray();
    }

    [Benchmark]
    public void Shuffle1_List_RandomShared()
    {
        var _ = Shuffle1(rangeArray, Random.Shared).ToArray();
    }

    [Benchmark]
    public void Shuffle1_Array_RandomShared()
    {
        var _ = Shuffle1(rangeArray, Random.Shared).ToArray();
    }
    #endregion

    #region Shuffle1_RandomInstance
    [Benchmark]
    public void Shuffle1_Enumerable_RandomInstance()
    {
        var _ = Shuffle1(rangeArray, new Random()).ToArray();
    }

    [Benchmark]
    public void Shuffle1_List_RandomInstance()
    {
        var _ = Shuffle1(rangeArray, new Random()).ToArray();
    }

    [Benchmark]
    public void Shuffle1_Array_RandomInstance()
    {
        var _ = Shuffle1(rangeArray, new Random()).ToArray();
    }
    #endregion

    #region Shuffle2_RandomShared
    [Benchmark]
    public void Shuffle2_Enumerable_RandomShared()
    {
        var _ = Shuffle2(rangeArray, Random.Shared).ToArray();
    }

    [Benchmark]
    public void Shuffle2_List_RandomShared()
    {
        var _ = Shuffle2(rangeArray, Random.Shared).ToArray();
    }

    [Benchmark]
    public void Shuffle2_Array_RandomShared()
    {
        var _ = Shuffle2(rangeArray, Random.Shared).ToArray();
    }
    #endregion

    #region Shuffle2_RandomInstance
    [Benchmark]
    public void Shuffle2_Enumerable_RandomInstance()
    {
        var _ = Shuffle2(rangeArray, new Random()).ToArray();
    }

    [Benchmark]
    public void Shuffle2_List_RandomInstance()
    {
        var _ = Shuffle2(rangeArray, new Random()).ToArray();
    }

    [Benchmark]
    public void Shuffle2_Array_RandomInstance()
    {
        var _ = Shuffle2(rangeArray, new Random()).ToArray();
    }
    #endregion

    #region NET8Shuffle
    [Benchmark]
    public void NET8Shuffle_Array_RandomShared()
    {
        Random.Shared.Shuffle(rangeArray);
    }

    [Benchmark]
    public void NET8Shuffle_Array_RandomInstance()
    {
        new Random().Shuffle(rangeArray);
    }
    #endregion

    #region Shuffle
    static IEnumerable<T> Shuffle1<T>(IEnumerable<T> source, Random rnd)
    {
        return source.OrderBy(_ => rnd);
    }

    static IEnumerable<T> Shuffle2<T>(IEnumerable<T> source, Random rnd)
    {
        var buffer = source.ToList();
        for (int i = 0; i < buffer.Count; i++)
        {
            int j = rnd.Next(i, buffer.Count);
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }
    #endregion
}