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
public class ClassBenchmark
{
    private const int _count = 10_000;

    public static readonly ClassA _classA1 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
    public static readonly ClassA _classA2 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

    public static readonly ClassB _classB1 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };
    public static readonly ClassB _classB2 = new() { Id = 100, Prop1 = 11, Prop2 = 22, Prop3 = 33/*, Prop4 = 44, Prop5 = 55*/ };

    [Benchmark(Description = "IEquatable<T>"), BenchmarkCategory("EqualityComparer<T>.Default")]
    public void Equals_Operator_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = EqualityComparer<ClassA>.Default.Equals(_classA1, _classA2);
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("EqualityComparer<T>.Default")]
    public void Equals_Operator_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = EqualityComparer<ClassB>.Default.Equals(_classB1, _classB2);
        }
    }

    [Benchmark(Description = "IEquatable<T>"), BenchmarkCategory("class.Equals")]
    public void Class_Equals_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _classA1.Equals(_classA2);
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("class.Equals")]
    public void Class_Equals_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _classB1.Equals(_classB2);
        }
    }

    [Benchmark(Description = "IEquatable<T>"), BenchmarkCategory("object.Equals")]
    public void Object_Equals_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = object.Equals(_classA1, _classA2);
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("object.Equals")]
    public void Object_Equals_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = object.Equals(_classB1, _classB2);
        }
    }

    [Benchmark(Description = "HashCode.Combine()"), BenchmarkCategory("GetHashCode")]
    public void Class_GetHashCode_A()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _classA1.GetHashCode();
        }
    }

    [Benchmark(Description = "usual"), BenchmarkCategory("GetHashCode")]
    public void Class_GetHashCode_B()
    {
        for (int i = 0; i < _count; i++)
        {
            var a = _classB1.GetHashCode();
        }
    }
}

public class ClassA : IEquatable<ClassA>
{
    public int Id { get; set; }
    public int Prop1 { get; set; }
    public int Prop2 { get; set; }
    public int Prop3 { get; set; }

    public override bool Equals(object obj)
    {
        return Equals(obj as ClassA);
    }

    public bool Equals(ClassA other)
    {
        return other is not null &&
               Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Prop1, Prop2, Prop3);
    }

    //public static bool operator ==(ClassA left, ClassA right)
    //{
    //    return left is null ? right is null : left.Equals(right); //is faster
    //    //return EqualityComparer<ClassA>.Default.Equals(left, right);
    //}

    //public static bool operator !=(ClassA left, ClassA right)
    //{
    //    return !(left == right);
    //}
}

public class ClassB
{
    public int Id { get; set; }
    public int Prop1 { get; set; }
    public int Prop2 { get; set; }
    public int Prop3 { get; set; }

    public override bool Equals(object obj)
    {
        return obj is ClassB other &&
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

    //public static bool operator ==(ClassB left, ClassB right)
    //{
    //    return EqualityComparer<ClassB>.Default.Equals(left, right);
    //}

    //public static bool operator !=(ClassB left, ClassB right)
    //{
    //    return !(left == right);
    //}
}