using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using BenchmarkDotNetVisualizer;

namespace Convert_Enum_To_String_Benchmark;

[RichImageExporter(title: "Converting Enum To A String Benchmarks", groupByColumns: ["Method"], spectrumColumns: ["Mean", "Allocated"])]
[RichHtmlExporter(title: "Converting Enum To A String Benchmarks", groupByColumns: ["Method"], spectrumColumns: ["Mean", "Allocated"])]
[RichMarkdownExporter(title: "Converting Enum To A String Benchmarks", groupByColumns: ["Method"], sortByColumns: ["Mean", "Allocated"])]


#if RELEASE
[ShortRunJob]
#endif

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