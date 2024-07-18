using System.Runtime.CompilerServices;

public static class StaticMapperOnlyTopLevelMethod
{
    #region Class
    [MethodImpl(MethodImplOptions.AggressiveInlining)] //Only top level methods has AggressiveInlining
    public static MyComplexClass MapAggressiveInlining(MyComplexClassDto source)
    {
        var target = new MyComplexClass();
        target.Int = source.Int;
        target.String = source.String;
        target.Boolean = source.Boolean;
        target.Long = source.Long;
        target.Double = source.Double;
        target.DateTime = source.DateTime;
        target.Enum = source.Enum;
        target.SubClass1 = MapSubClass1(source.SubClass1);
        return target;
    }

    private static MySubClass1 MapSubClass1(MySubClass1Dto source)
    {
        var target = new MySubClass1();
        target.Int = source.Int;
        target.String = source.String;
        target.SubClass2 = MapSubClass2(source.SubClass2);
        return target;
    }

    private static MySubClass2 MapSubClass2(MySubClass2Dto source)
    {
        var target = new MySubClass2();
        target.Int = source.Int;
        target.String = source.String;
        target.SubClass3 = MapSubClass3(source.SubClass3);
        return target;
    }

    private static MySubClass3 MapSubClass3(MySubClass3Dto source)
    {
        var target = new MySubClass3();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }
    #endregion

    #region Struct
    [MethodImpl(MethodImplOptions.AggressiveInlining)] //Only top level methods has AggressiveInlining
    public static MyComplexStruct MapAggressiveInlining(MyComplexStructDto source)
    {
        var target = new MyComplexStruct();
        target.Int = source.Int;
        target.String = source.String;
        target.Boolean = source.Boolean;
        target.Long = source.Long;
        target.Double = source.Double;
        target.DateTime = source.DateTime;
        target.Enum = source.Enum;
        target.SubStruct1 = MapSubStruct1(source.SubStruct1);
        return target;
    }

    private static MySubStruct1 MapSubStruct1(MySubStruct1Dto source)
    {
        var target = new MySubStruct1();
        target.Int = source.Int;
        target.String = source.String;
        target.SubStruct2 = MapSubStruct2(source.SubStruct2);
        return target;
    }

    private static MySubStruct2 MapSubStruct2(MySubStruct2Dto source)
    {
        var target = new MySubStruct2();
        target.Int = source.Int;
        target.String = source.String;
        target.SubStruct3 = MapSubStruct3(source.SubStruct3);
        return target;
    }

    private static MySubStruct3 MapSubStruct3(MySubStruct3Dto source)
    {
        var target = new MySubStruct3();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }
    #endregion
}