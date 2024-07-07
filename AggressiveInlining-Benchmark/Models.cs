namespace Models;

#region Class
public class MyClass
{
    public int Int { get; set; }
    public string String { get; set; }
    public bool Boolean { get; set; }
    public long Long { get; set; }
    public double Double { get; set; }
    public DateTime DateTime { get; set; }
    public MyEnum Enum { get; set; }
    public MySubClass SubClass { get; set; }
}

public class MySubClass
{
    public int Int { get; set; }
    public string String { get; set; }
}

public class MyClassDto
{
    public int Int { get; set; }
    public string String { get; set; }
    public bool Boolean { get; set; }
    public long Long { get; set; }
    public double Double { get; set; }
    public DateTime DateTime { get; set; }
    public MyEnum Enum { get; set; }
    public MySubClassDto SubClass { get; set; }
}

public class MySubClassDto
{
    public int Int { get; set; }
    public string String { get; set; }
}
#endregion

#region Struct
public struct MyStruct
{
    public int Int { get; set; }
    public string String { get; set; }
    public bool Boolean { get; set; }
    public long Long { get; set; }
    public double Double { get; set; }
    public DateTime DateTime { get; set; }
    public MyEnum Enum { get; set; }
    public MySubStruct SubStruct { get; set; }
}

public struct MySubStruct
{
    public int Int { get; set; }
    public string String { get; set; }
}

public struct MyStructDto
{
    public int Int { get; set; }
    public string String { get; set; }
    public bool Boolean { get; set; }
    public long Long { get; set; }
    public double Double { get; set; }
    public DateTime DateTime { get; set; }
    public MyEnum Enum { get; set; }
    public MySubStructDto SubStruct { get; set; }
}

public struct MySubStructDto
{
    public int Int { get; set; }
    public string String { get; set; }
}
#endregion

public enum MyEnum
{
    Enum1,
    Enum2,
    Enum3
}
