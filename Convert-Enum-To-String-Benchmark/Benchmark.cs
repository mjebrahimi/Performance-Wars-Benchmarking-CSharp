using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EnumFastToStringGenerated;
using EnumsNET;
using EnumStringValues;
using EnumUtils;
using FastEnumUtility;

namespace Convert_Enum_To_String_Benchmark;

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif

[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class Benchmark
{
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable RCS1015 // Use nameof operator

    [Benchmark(Description = "ToString()"), BenchmarkCategory(".Net Built-In")]
    public string UsingToString()
    {
        return MyEnum.ALongAndVerboseEnumName.ToString();
    }

    [Benchmark(Description = "nameof()"), BenchmarkCategory(".Net Built-In")]
    public string UsingNameOf()
    {
        return nameof(MyEnum.ALongAndVerboseEnumName);
    }

    [Benchmark(Description = "Enum.GetName() (Generic)"), BenchmarkCategory(".Net Built-In")]
    public string UsingEnumGetName1()
    {
        return Enum.GetName(MyEnum.ALongAndVerboseEnumName)!;
    }

    [Benchmark(Description = "Enum.GetName() (Non Generic)"), BenchmarkCategory(".Net Built-In")]
    public string UsingEnumGetName2()
    {
        return Enum.GetName(typeof(MyEnum), MyEnum.ALongAndVerboseEnumName)!;
    }

    [Benchmark(Description = "ToStringFast()"), BenchmarkCategory("Supernova.Enum.Generators")]
    public string UsingSupernovaToStringFast()
    {
        return MyEnum2.ALongAndVerboseEnumName.ToStringFast();
    }

    [Benchmark(Description = "AsString()"), BenchmarkCategory("Enums.NET")]
    public string UsingAsString()
    {
        return MyEnum.ALongAndVerboseEnumName.AsString();
    }

    [Benchmark(Description = "FastToString()"), BenchmarkCategory("FastEnum")]
    public string UsingFastToString()
    {
        return MyEnum.ALongAndVerboseEnumName.FastToString();
    }

    [Benchmark(Description = "GetName()"), BenchmarkCategory("EnumUtils")]
    public string UsingGetName()
    {
        return EnumHelper.GetName(MyEnum.ALongAndVerboseEnumName);
    }

    [Benchmark(Description = "ToStringFast"), BenchmarkCategory("NetEscapades.EnumGenerators")]
    public string UsingNetEscapadesToStringFast()
    {
        return MyEnum3.ALongAndVerboseEnumName.ToStringFast();
    }

    [Benchmark(Description = "GetStringValue()"), BenchmarkCategory("EnumStringValues")]
    public string UsingGetStringValue()
    {
        return MyEnum.ALongAndVerboseEnumName.GetStringValue();
    }

#pragma warning restore RCS1015 // Use nameof operator
#pragma warning restore CA1822 // Mark members as static
}