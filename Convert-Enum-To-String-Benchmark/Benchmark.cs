using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Convert_Enum_To_String_Benchmark;

#if RELEASE
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)] //more accurate results
#endif
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class Benchmark
{
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable RCS1015 // Use nameof operator
    [Benchmark(Description = "ToString()")]
    public string UsingToString()
    {
        return MyEnum.ALongAndVerboseEnumName.ToString();
    }

    [Benchmark(Description = "nameof()")]
    public string UsingNameOf()
    {
        return nameof(MyEnum.ALongAndVerboseEnumName);
    }

    [Benchmark(Description = "Enum.GetName() (Generic)")]
    public string UsingEnumGetName1()
    {
        return Enum.GetName(MyEnum.ALongAndVerboseEnumName)!;
    }

    [Benchmark(Description = "Enum.GetName() (Non Generic)")]
    public string UsingEnumGetName2()
    {
        return Enum.GetName(typeof(MyEnum), MyEnum.ALongAndVerboseEnumName)!;
    }
#pragma warning restore RCS1015 // Use nameof operator
#pragma warning restore CA1822 // Mark members as static
}