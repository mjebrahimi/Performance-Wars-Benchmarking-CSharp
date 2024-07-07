using Models;
using System.Runtime.CompilerServices;

public static class Mapper
{
    #region Class
    public static MyClass Map(MyClassDto source)
    {
        var target = new MyClass();
        target.Int = source.Int;
        target.String = source.String;
        target.Boolean = source.Boolean;
        target.Long = source.Long;
        target.Double = source.Double;
        target.DateTime = source.DateTime;
        target.Enum = source.Enum;
        target.SubClass = MapSubClass(source.SubClass);
        return target;
    }

    private static MySubClass MapSubClass(MySubClassDto source)
    {
        var target = new MySubClass();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyClass MapAggressiveInlining(MyClassDto source)
    {
        var target = new MyClass();
        target.Int = source.Int;
        target.String = source.String;
        target.Boolean = source.Boolean;
        target.Long = source.Long;
        target.Double = source.Double;
        target.DateTime = source.DateTime;
        target.Enum = source.Enum;
        target.SubClass = MapSubClassAggressiveInlining(source.SubClass);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubClass MapSubClassAggressiveInlining(MySubClassDto source)
    {
        var target = new MySubClass();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }
    #endregion

    #region Struct
    public static MyStruct Map(MyStructDto source)
    {
        var target = new MyStruct();
        target.Int = source.Int;
        target.String = source.String;
        target.Boolean = source.Boolean;
        target.Long = source.Long;
        target.Double = source.Double;
        target.DateTime = source.DateTime;
        target.Enum = source.Enum;
        target.SubStruct = MapSubStruct(source.SubStruct);
        return target;
    }

    private static MySubStruct MapSubStruct(MySubStructDto source)
    {
        var target = new MySubStruct();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyStruct MapAggressiveInlining(MyStructDto source)
    {
        var target = new MyStruct();
        target.Int = source.Int;
        target.String = source.String;
        target.Boolean = source.Boolean;
        target.Long = source.Long;
        target.Double = source.Double;
        target.DateTime = source.DateTime;
        target.Enum = source.Enum;
        target.SubStruct = MapSubStructAggressiveInlining(source.SubStruct);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubStruct MapSubStructAggressiveInlining(MySubStructDto source)
    {
        var target = new MySubStruct();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }
    #endregion
}