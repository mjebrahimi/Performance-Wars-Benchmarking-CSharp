using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

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
    private readonly int[] _array = new int[] { 5, 10 };
    private readonly List<int> _list = new() { 5, 10 };

    [Benchmark]
    public void Array_OrderBy_ToArray_01()
    {
        var ordered = _array.OrderBy(p => p).ToArray();
        var min = ordered[0];
        var max = ordered[1];
    }

    [Benchmark]
    public void Array_OrderBy_ToArray_FirstLast()
    {
        var ordered = _array.OrderBy(p => p).ToArray();
        var min = ordered.First();
        var max = ordered.Last();
    }

    [Benchmark]
    public void Array_OrderBy_Enumerable_01()
    {
        var ordered = _array.OrderBy(p => p);
        var min = ordered.ElementAt(0);
        var max = ordered.ElementAt(1);
    }

    [Benchmark]
    public void Array_OrderBy_Enumerable_FirstLast()
    {
        var ordered = _array.OrderBy(p => p);
        var min = ordered.First();
        var max = ordered.Last();
    }

    [Benchmark]
    public void Array_MinMax()
    {
        var min = _array.Min();
        var max = _array.Max();
    }

    [Benchmark]
    public void List__OrderBy_ToList__01()
    {
        var ordered = _list.OrderBy(p => p).ToList();
        var min = ordered[0];
        var max = ordered[1];
    }

    [Benchmark]
    public void List__OrderBy_ToList__FirstLast()
    {
        var ordered = _list.OrderBy(p => p).ToList();
        var min = ordered.First();
        var max = ordered.Last();
    }

    [Benchmark]
    public void List__OrderBy_Enumerable_01()
    {
        var ordered = _list.OrderBy(p => p);
        var min = ordered.ElementAt(0);
        var max = ordered.ElementAt(1);
    }

    [Benchmark]
    public void List__OrderBy_Enumerable_FirstLast()
    {
        var ordered = _list.OrderBy(p => p);
        var min = ordered.First();
        var max = ordered.Last();
    }

    [Benchmark]
    public void List__MinMax()
    {
        var min = _list.Min();
        var max = _list.Max();
    }
}