using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNetVisualizer;
using Models;

var summary = BenchmarkAutoRunner.Run<Benchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "AggressiveInlining Benchmark",
        GroupByColumns = ["Categories"],
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = true,
    });

Console.ReadLine();

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
public class Benchmark
{
    public static readonly MyStructDto myStructDto = new MyStructDto
    {
        Int = 100,
        String = "String",
        Boolean = true,
        Long = 100,
        Double = 100.0,
        DateTime = DateTime.Now,
        Enum = MyEnum.Enum1,
        SubStruct = new MySubStructDto
        {
            Int = 100,
            String = "String"
        }
    };

    public static readonly MyClassDto myClassDto = new MyClassDto
    {
        Int = 100,
        String = "String",
        Boolean = true,
        Long = 100,
        Double = 100.0,
        DateTime = DateTime.Now,
        Enum = MyEnum.Enum1,
        SubClass = new MySubClassDto
        {
            Int = 100,
            String = "String"
        }
    };

    #region Class
    [Benchmark(Description = "MapAggressiveInlining"), BenchmarkCategory("Class")]
    public MyClass MapAggressiveInlining_Class() => Mapper.MapAggressiveInlining(myClassDto);

    [Benchmark(Description = "Map"), BenchmarkCategory("Class")]
    public MyClass Map_Class() => Mapper.Map(myClassDto);
    #endregion

    #region Struct
    [Benchmark(Description = "MapAggressiveInlining"), BenchmarkCategory("Struct")]
    public MyStruct MapAggressiveInlining_Struct() => Mapper.MapAggressiveInlining(myStructDto);

    [Benchmark(Description = "Map"), BenchmarkCategory("Struct")]
    public MyStruct Map_Struct() => Mapper.Map(myStructDto);
    #endregion
}
