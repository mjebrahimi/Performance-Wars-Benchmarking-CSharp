using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNetVisualizer;

var summary = BenchmarkAutoRunner.Run<Benchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "Benchmark of different ways to Instantiate an object",
        GroupByColumns = ["Categories"],
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = true,
    });

Console.ReadLine();

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
//[ShortRunJob]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
public class Benchmark
{
    #region Class
    [Benchmark(Description = "new()"), BenchmarkCategory("Class")]
    public MyClass new_Class() => new();
    [Benchmark(Description = "new<T>()"), BenchmarkCategory("Class")]
    public MyClass NewT_Class() => Factory<MyClass>.New();

    [Benchmark(Description = "ActivatorCreateInstance"), BenchmarkCategory("Class")]
    public MyClass ActivatorCreateInstance_Class() => Factory<MyClass>.ActivatorCreateInstance();

    [Benchmark(Description = "Reflection"), BenchmarkCategory("Class")]
    public MyClass Reflection_Class() => Factory<MyClass>.Reflection();

    [Benchmark(Description = "ReflectionEmit"), BenchmarkCategory("Class")]
    public MyClass ReflectionEmit_Class() => Factory<MyClass>.ReflectionEmit();

    [Benchmark(Description = "CompiledExpression"), BenchmarkCategory("Class")]
    public MyClass CompiledExpression_Class() => Factory<MyClass>.CompiledExpression();
    #endregion

    #region Struct
    [Benchmark(Description = "new()"), BenchmarkCategory("Struct")]
    public MyStruct new_Struct() => new();

    [Benchmark(Description = "new<T>()"), BenchmarkCategory("Struct")]
    public MyStruct NewT_Struct() => Factory<MyStruct>.New();

    [Benchmark(Description = "ActivatorCreateInstance"), BenchmarkCategory("Struct")]
    public MyStruct ActivatorCreateInstance_Struct() => Factory<MyStruct>.ActivatorCreateInstance();

    [Benchmark(Description = "Reflection"), BenchmarkCategory("Struct")]
    public MyStruct Reflection_Struct() => Factory<MyStruct>.Reflection();

    [Benchmark(Description = "ReflectionEmit"), BenchmarkCategory("Struct")]
    public MyStruct ReflectionEmit_Struct() => Factory<MyStruct>.ReflectionEmit();

    [Benchmark(Description = "CompiledExpression"), BenchmarkCategory("Struct")]
    public MyStruct CompiledExpression_Struct() => Factory<MyStruct>.CompiledExpression();
    #endregion
}
