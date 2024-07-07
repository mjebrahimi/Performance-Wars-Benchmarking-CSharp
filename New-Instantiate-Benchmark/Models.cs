public class MyClass()
{
    public int Int { get; set; }
    public MyEnum Enum { get; set; }
    public string Name { get; set; } = null!;
    public bool Boolean { get; set; }
    public long Long { get; set; }
    public decimal Decimal { get; set; }
    public double Double { get; set; }
    public DateTime DateTime { get; set; }
    public List<int> List { get; set; } = null!;
    public Dictionary<string, int> Dictionary { get; set; } = null!;
}

public struct MyStruct()
{
    public int Int { get; set; }
    public MyEnum Enum { get; set; }
    public string Name { get; set; } = null!;
    public bool Boolean { get; set; }
    public long Long { get; set; }
    public decimal Decimal { get; set; }
    public double Double { get; set; }
    public DateTime DateTime { get; set; }
    public List<int> List { get; set; } = null!;
    public Dictionary<string, int> Dictionary { get; set; } = null!;
}

public enum MyEnum
{
    Enum1,
    Enum2,
    Enum3
}