using EnumFastToStringGenerated;

namespace Convert_Enum_To_String_Benchmark;



#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
public enum MyEnum
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
{
    ALongAndVerboseEnumName
}

[EnumGenerator]
public enum MyEnum2
{
   ALongAndVerboseEnumName
}
