using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNetVisualizer;
using Models;

BenchmarkAutoRunner.Run<Benchmark>(args);

Console.ReadLine();

#if RELEASE
//[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
[ShortRunJob]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
public class Benchmark
{
    public static readonly MyStructDto myStructDto = new MyStructDto
    {
        AlbumType = "AlbumType",
        Href = "Href",
        Id = "Id",
        Name = "Name",
        Popularity = 100,
        ReleaseDate = "ReleaseDate",
        ReleaseDatePrecision = "ReleaseDatePrecision",
        Type = "Type",
        Uri = "Uri",
        Track = new MySubStructDto
        {
            Href = "Href",
            Limit = 100,
            Offset = 100,
            Total = 100
        }
    };

    public static readonly MyClassDto myClassDto = new MyClassDto
    {
        AlbumType = "AlbumType",
        Href = "Href",
        Id = "Id",
        Name = "Name",
        Popularity = 100,
        ReleaseDate = "ReleaseDate",
        ReleaseDatePrecision = "ReleaseDatePrecision",
        Type = "Type",
        Uri = "Uri",
        Track = new MySubClassDto
        {
            Href = "Href",
            Limit = 100,
            Offset = 100,
            Total = 100
        }
    };

    #region Class
    [Benchmark(Description = "MapSimplified_AggressiveInlining"), BenchmarkCategory("Class")]
    public MyClass MapSimplified_AggressiveInlining_Class() => Mapper.MapSimplified_AggressiveInlining(myClassDto);

    [Benchmark(Description = "MapSimplified_NoAggressiveInlining"), BenchmarkCategory("Class")]
    public MyClass MapSimplified_NoAggressiveInlining_Class() => Mapper.MapSimplified_NoAggressiveInlining(myClassDto);

    [Benchmark(Description = "Map_AggressiveInlining"), BenchmarkCategory("Class")]
    public MyClass Map_AggressiveInlining_Class() => Mapper.Map_AggressiveInlining(myClassDto);

    [Benchmark(Description = "Map_NoAggressiveInlining"), BenchmarkCategory("Class")]
    public MyClass Map_NoAggressiveInlining_Class() => Mapper.Map_NoAggressiveInlining(myClassDto);
    #endregion

    #region Struct
    [Benchmark(Description = "MapSimplified_AggressiveInlining"), BenchmarkCategory("Struct")]
    public MyStruct MapSimplified_AggressiveInlining_Struct() => Mapper.MapSimplified_AggressiveInlining(myStructDto);

    [Benchmark(Description = "MapSimplified_NoAggressiveInlining"), BenchmarkCategory("Struct")]
    public MyStruct MapSimplified_NoAggressiveInlining_Struct() => Mapper.MapSimplified_NoAggressiveInlining(myStructDto);

    [Benchmark(Description = "Map_AggressiveInlining"), BenchmarkCategory("Struct")]
    public MyStruct Map_AggressiveInlining_Struct() => Mapper.Map_AggressiveInlining(myStructDto);

    [Benchmark(Description = "Map_NoAggressiveInlining"), BenchmarkCategory("Struct")]
    public MyStruct Map_NoAggressiveInlining_Struct() => Mapper.Map_NoAggressiveInlining(myStructDto);
    #endregion

    #region Comment
    //public static readonly MyStructDto myStructDto = new MyStructDto
    //{
    //    Int = 100,
    //    Boolean = true,
    //    DateTime = DateTime.Now,
    //    Decimal = 100.0m,
    //    Double = 100.0,
    //    Enum = MyEnum.Enum1,
    //    Long = 100,
    //    String = "String",
    //    SubStruct = new MySubStructDto
    //    {
    //        Int = 100,
    //        String = "String"
    //    }
    //};

    //public static readonly MyClassDto myClassDto = new MyClassDto
    //{
    //    Int = 100,
    //    Boolean = true,
    //    DateTime = DateTime.Now,
    //    Decimal = 100.0m,
    //    Double = 100.0,
    //    Enum = MyEnum.Enum1,
    //    Long = 100,
    //    String = "String",
    //    SubClass = new MySubClassDto
    //    {
    //        Int = 100,
    //        String = "String"
    //    }
    //};

    //#region Class
    //[Benchmark, BenchmarkCategory("Class")]
    //public MyClass MapSimplifiedAggressiveInlining_Class() => Mapper.MapSimplifiedAggressiveInlining(myClassDto);

    //[Benchmark, BenchmarkCategory("Class")]
    //public MyClass MapAggressiveInlining_Class() => Mapper.MapAggressiveInlining(myClassDto);

    //[Benchmark, BenchmarkCategory("Class")]
    //public MyClass MapSimplified_Class() => Mapper.MapSimplified(myClassDto);

    //[Benchmark, BenchmarkCategory("Class")]
    //public MyClass MapNoAggressiveInlining_Class() => Mapper.MapNoAggressiveInlining(myClassDto);
    //#endregion

    //#region Struct
    //[Benchmark, BenchmarkCategory("Struct")]
    //public MyStruct MapSimplifiedAggressiveInlining_Struct() => Mapper.MapSimplifiedAggressiveInlining(myStructDto);

    //[Benchmark, BenchmarkCategory("Struct")]
    //public MyStruct MapAggressiveInlining_Struct() => Mapper.MapAggressiveInlining(myStructDto);

    //[Benchmark, BenchmarkCategory("Struct")]
    //public MyStruct MapSimplified_Struct() => Mapper.MapSimplified(myStructDto);

    //[Benchmark, BenchmarkCategory("Struct")]
    //public MyStruct MapNoAggressiveInlining_Struct() => Mapper.MapNoAggressiveInlining(myStructDto);
    //#endregion
    #endregion
}
