using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System.Buffers;

#if DEBUG

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("***** To achieve accurate results, set project configuration to Release mode. *****");

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("***** Waite 3 seconds for DEBUG MODE! *****");

Thread.Sleep(3000);

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig()); //For Debugging
BenchmarkRunner.Run<ArrayBenchmark>(new BenchmarkDotNet.Configs.DebugInProcessConfig());

#else

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
BenchmarkRunner.Run<ArrayBenchmark>();

#endif

Console.ReadLine();


#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[MemoryDiagnoser(displayGenColumns: false)]
[KeepBenchmarkFiles(false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class ArrayBenchmark
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int ArraySize { get; set; }

    [Benchmark(Baseline = true)]
    public int[] NewArray() => new int[ArraySize];

    [Benchmark]
    public int[] ArrayPoolRent() => ArrayPool<int>.Shared.Rent(ArraySize);

    [Benchmark]
    public int[] GCZeroInitialized() => GC.AllocateArray<int>(ArraySize);

    [Benchmark]
    public int[] GCZeroUninitialized() => GC.AllocateUninitializedArray<int>(ArraySize);

    [Benchmark]
    public int[] ArrayCreateInstance() => (int[])Array.CreateInstance(typeof(int), ArraySize);
}