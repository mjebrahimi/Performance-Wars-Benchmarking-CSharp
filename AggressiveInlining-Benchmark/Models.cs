namespace Models;

#region Class
public class MyClass
{
    public string AlbumType { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public long Popularity { get; set; }
    public string ReleaseDate { get; set; }
    public string ReleaseDatePrecision { get; set; }
    public MySubClass Track { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
}

public class MySubClass
{
    public string Href { get; set; }
    public long Limit { get; set; }
    public long Offset { get; set; }
    public long Total { get; set; }
}

public class MyClassDto
{
    public string AlbumType { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public long Popularity { get; set; }
    public string ReleaseDate { get; set; }
    public string ReleaseDatePrecision { get; set; }
    public MySubClassDto Track { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
}

public class MySubClassDto
{
    public string Href { get; set; }
    public long Limit { get; set; }
    public long Offset { get; set; }
    public long Total { get; set; }
}
#endregion

#region Struct
public struct MyStruct
{
    public string AlbumType { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public long Popularity { get; set; }
    public string ReleaseDate { get; set; }
    public string ReleaseDatePrecision { get; set; }
    public MySubStruct Track { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
}

public struct MySubStruct
{
    public string Href { get; set; }
    public long Limit { get; set; }
    public long Offset { get; set; }
    public long Total { get; set; }
}

public struct MyStructDto
{
    public string AlbumType { get; set; }
    public string Href { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public long Popularity { get; set; }
    public string ReleaseDate { get; set; }
    public string ReleaseDatePrecision { get; set; }
    public MySubStructDto Track { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
}

public struct MySubStructDto
{
    public string Href { get; set; }
    public long Limit { get; set; }
    public long Offset { get; set; }
    public long Total { get; set; }
}
#endregion

#region Comment
//#region Class
//public class MyClass
//{
//    public int Int { get; set; }
//    public MyEnum Enum { get; set; }
//    public string String { get; set; }
//    public bool Boolean { get; set; }
//    public long Long { get; set; }
//    public decimal Decimal { get; set; }
//    public double Double { get; set; }
//    public DateTime DateTime { get; set; }
//    public MySubClass SubClass { get; set; }
//}

//public class MySubClass
//{
//    public int Int { get; set; }
//    public string String { get; set; }
//}

//public class MyClassDto
//{
//    public int Int { get; set; }
//    public MyEnum Enum { get; set; }
//    public string String { get; set; }
//    public bool Boolean { get; set; }
//    public long Long { get; set; }
//    public decimal Decimal { get; set; }
//    public double Double { get; set; }
//    public DateTime DateTime { get; set; }
//    public MySubClassDto SubClass { get; set; }
//}

//public class MySubClassDto
//{
//    public int Int { get; set; }
//    public string String { get; set; }
//}
//#endregion

//#region Struct
//public struct MyStruct
//{
//    public int Int { get; set; }
//    public MyEnum Enum { get; set; }
//    public string String { get; set; }
//    public bool Boolean { get; set; }
//    public long Long { get; set; }
//    public decimal Decimal { get; set; }
//    public double Double { get; set; }
//    public DateTime DateTime { get; set; }
//    public MySubStruct SubStruct { get; set; }
//}

//public struct MySubStruct
//{
//    public int Int { get; set; }
//    public string String { get; set; }
//}

//public struct MyStructDto
//{
//    public int Int { get; set; }
//    public MyEnum Enum { get; set; }
//    public string String { get; set; }
//    public bool Boolean { get; set; }
//    public long Long { get; set; }
//    public decimal Decimal { get; set; }
//    public double Double { get; set; }
//    public DateTime DateTime { get; set; }
//    public MySubStructDto SubStruct { get; set; }
//}

//public struct MySubStructDto
//{
//    public int Int { get; set; }
//    public string String { get; set; }
//}
//#endregion

//public enum MyEnum
//{
//    Enum1,
//    Enum2,
//    Enum3
//}
#endregion
