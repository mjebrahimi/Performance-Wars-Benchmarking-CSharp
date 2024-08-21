using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
[DisplayName("UnsafeAccessor w/wo AggressiveInlining Benchmark")]
public class UnsafeAccessorBenchmark
{
    private static Class _class = new();
    private static Struct _struct = new();

    [Benchmark(Description = "UnsafeAccessor"), BenchmarkCategory("Class")]
    public int Class_UnsafeAccessor() => MethodCaller.Class_PrivateMethod(_class, 42);

    [Benchmark(Description = "UnsafeAccessorWithAggressiveInlining"), BenchmarkCategory("Class")]
    public int Class_UnsafeAccessorWithInlining() => MethodCaller.Class_PrivateMethodWithInlining(_class, 42);

    [Benchmark(Description = "UnsafeAccessor"), BenchmarkCategory("Struct")]
    public int Struct_UnsafeAccessor() => MethodCaller.Struct_PrivateMethod(ref _struct, 42);

    [Benchmark(Description = "UnsafeAccessorWithAggressiveInlining"), BenchmarkCategory("Struct")]
    public int Struct_UnsafeAccessorWithInlining() => MethodCaller.Struct_PrivateMethodWithInlining(ref _struct, 42);
}

public static class MethodCaller
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "PrivateMethod")]
    public static extern int Class_PrivateMethod(this Class @this, int value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "PrivateMethod")]
    public static extern int Class_PrivateMethodWithInlining(this Class @this, int value);

    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "PrivateMethod")]
    public static extern int Struct_PrivateMethod(ref Struct @this, int value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "PrivateMethod")]
    public static extern int Struct_PrivateMethodWithInlining(ref Struct @this, int value);
}

public class Class
{
    private int PrivateMethod(int value) => value;
}

public struct Struct
{
    private int PrivateMethod(int value) => value;
}