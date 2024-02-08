using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;

namespace Convert_Enum_To_String_Benchmark;

[SimpleJob(RunStrategy.Throughput)]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class Benchmark
{
    [Benchmark(Description = "ToString()")]
    public string ConvertUsingToStringMethod()
    {
        return MyEnum.ALongAndVerboseEnumName.ToString();
    }

    [Benchmark(Description = "NameOf()")]
    public string ConvertUsingNameOfMethod() 
    {
        return nameof(MyEnum.ALongAndVerboseEnumName);
    }

    [Benchmark(Description = "Enum_GetName()_FirstOverload")]
    public string ConvertUsingGetNameMethod()
    {
        return Enum.GetName(MyEnum.ALongAndVerboseEnumName)!;
    }
    
    [Benchmark(Description = "Enum_GetName()_SecondOverload")]
    public string ConvertUsingSecondOverloadOfGetNameMethod()
    {
        return Enum.GetName(typeof(MyEnum), MyEnum.ALongAndVerboseEnumName)!;
    }
}