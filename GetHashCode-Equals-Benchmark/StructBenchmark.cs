using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

#if RELEASE
//[DryJob] //Don't use for real benchmark (Just for Test)
//[ShortRunJob]
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[CategoriesColumn]
[MemoryDiagnoser(displayGenColumns: false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByCategory)]
public class StructBenchmark
{
    private const int _count = 10_000;

    public static readonly StructA _structA1 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
    public static readonly StructA _structA2 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

    public static readonly StructB _structB1 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
    public static readonly StructB _structB2 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

    [Benchmark(Description = "IEquatable<T>"), BenchmarkCategory("EqualityComparer<T>.Default")]
    public void Equals_Operator_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = EqualityComparer<StructA>.Default.Equals(_structA1, _structA2);
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("EqualityComparer<T>.Default")]
    public void Equals_Operator_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = EqualityComparer<StructB>.Default.Equals(_structB1, _structB2);
        }
    }

    [Benchmark(Description = "IEquatable<T>"), BenchmarkCategory("struct.Equals")]
    public void Struct_Equals_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _structA1.Equals(_structA2);
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("struct.Equals")]
    public void Struct_Equals_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _structB1.Equals(_structB2);
        }
    }

    [Benchmark(Description = "IEquatable<T>"), BenchmarkCategory("object.Equals")]
    public void Object_Equals_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = object.Equals(_structA1, _structA2);
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("object.Equals")]
    public void Object_Equals_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = object.Equals(_structB1, _structB2);
        }
    }

    [Benchmark(Description = "HashCode.Combine()"), BenchmarkCategory("GetHashCode")]
    public void Struct_GetHashCode_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _structA1.GetHashCode();
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("GetHashCode")]
    public void Struct_GetHashCode_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _structB1.GetHashCode();
        }
    }
}

public struct StructA : IEquatable<StructA>
{
    public int Id { get; set; }
    public int Prop1 { get; set; }
    public int Prop2 { get; set; }
    public int Prop3 { get; set; }

    public override bool Equals(object obj)
    {
        return Equals((StructA)obj);
    }

    public bool Equals(StructA other)
    {
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Prop1, Prop2, Prop3);
    }

    //public static bool operator ==(StructA left, StructA right)
    //{
    //    return left.Equals(right); //is faster
    //    //return EqualityComparer<StructA>.Default.Equals(left, right);
    //}

    //public static bool operator !=(StructA left, StructA right)
    //{
    //    return !(left == right);
    //}
}

public struct StructB
{
    public int Id { get; set; }
    public int Prop1 { get; set; }
    public int Prop2 { get; set; }
    public int Prop3 { get; set; }

    public override bool Equals(object obj)
    {
        return obj is StructB other &&
               Id == other.Id;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 13;
            hash = (hash * 7) + Prop1.GetHashCode();
            hash = (hash * 7) + Prop2.GetHashCode();
            hash = (hash * 7) + Prop3.GetHashCode();
            return hash;
        }
    }

    //public static bool operator ==(StructB left, StructB right)
    //{
    //    return EqualityComparer<StructB>.Default.Equals(left, right);
    //}

    //public static bool operator !=(StructB left, StructB right)
    //{
    //    return !(left == right);
    //}
}