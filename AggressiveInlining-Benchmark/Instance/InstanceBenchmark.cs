using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.ComponentModel;

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
[DisplayName("AggressiveInlining Benchmark (instance mapper)")]
public class InstanceBenchmark
{
    private static readonly InstanceMapper _mapper = new();

    private static readonly MyComplexStructDto myComplexStructDto = new()
    {
        Int = 100,
        String = "String",
        Boolean = true,
        Long = 100,
        Double = 100.0,
        DateTime = DateTime.Now,
        Enum = MyEnum.Enum1,
        SubStruct1 = new MySubStruct1Dto
        {
            Int = 101,
            String = "SubStruct1",
            SubStruct2 = new MySubStruct2Dto
            {
                Int = 102,
                String = "SubStruct2",
                SubStruct3 = new MySubStruct3Dto
                {
                    Int = 103,
                    String = "SubStruct3"
                }
            }
        }
    };

    private static readonly MyComplexClassDto myComplexClassDto = new()
    {
        Int = 100,
        String = "String",
        Boolean = true,
        Long = 100,
        Double = 100.0,
        DateTime = DateTime.Now,
        Enum = MyEnum.Enum1,
        SubClass1 = new MySubClass1Dto
        {
            Int = 101,
            String = "SubClass1",
            SubClass2 = new MySubClass2Dto
            {
                Int = 102,
                String = "SubClass2",
                SubClass3 = new MySubClass3Dto
                {
                    Int = 103,
                    String = "SubClass3"
                }
            }
        }
    };

    #region Class
    [Benchmark(Description = "MapAggressiveInlining"), BenchmarkCategory("Class")]
    public MyComplexClass MapAggressiveInlining_Class() => _mapper.MapAggressiveInlining(myComplexClassDto);

    [Benchmark(Description = "Map"), BenchmarkCategory("Class")]
    public MyComplexClass Map_Class() => _mapper.Map(myComplexClassDto);
    #endregion

    #region Struct
    [Benchmark(Description = "MapAggressiveInlining"), BenchmarkCategory("Struct")]
    public MyComplexStruct MapAggressiveInlining_Struct() => _mapper.MapAggressiveInlining(myComplexStructDto);

    [Benchmark(Description = "Map"), BenchmarkCategory("Struct")]
    public MyComplexStruct Map_Struct() => _mapper.Map(myComplexStructDto);
    #endregion
}
