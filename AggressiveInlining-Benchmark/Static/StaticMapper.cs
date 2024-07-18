using System.Runtime.CompilerServices;

public static class StaticMapper
{
    #region Class
    #region Without AggressiveInlining
    public static MyComplexClass Map(MyComplexClassDto source)
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

    #region With AggressiveInlining
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        target.SubClass1 = MapSubClass1AggressiveInlining(source.SubClass1);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubClass1 MapSubClass1AggressiveInlining(MySubClass1Dto source)
    {
        var target = new MySubClass1();
        target.Int = source.Int;
        target.String = source.String;
        target.SubClass2 = MapSubClass2AggressiveInlining(source.SubClass2);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubClass2 MapSubClass2AggressiveInlining(MySubClass2Dto source)
    {
        var target = new MySubClass2();
        target.Int = source.Int;
        target.String = source.String;
        target.SubClass3 = MapSubClass3AggressiveInlining(source.SubClass3);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubClass3 MapSubClass3AggressiveInlining(MySubClass3Dto source)
    {
        var target = new MySubClass3();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }
    #endregion
    #endregion

    #region Struct
    #region Without AggressiveInlining
    public static MyComplexStruct Map(MyComplexStructDto source)
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

    #region With AggressiveInlining
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        target.SubStruct1 = MapSubStruct1AggressiveInlining(source.SubStruct1);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubStruct1 MapSubStruct1AggressiveInlining(MySubStruct1Dto source)
    {
        var target = new MySubStruct1();
        target.Int = source.Int;
        target.String = source.String;
        target.SubStruct2 = MapSubStruct2AggressiveInlining(source.SubStruct2);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubStruct2 MapSubStruct2AggressiveInlining(MySubStruct2Dto source)
    {
        var target = new MySubStruct2();
        target.Int = source.Int;
        target.String = source.String;
        target.SubStruct3 = MapSubStruct3AggressiveInlining(source.SubStruct3);
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubStruct3 MapSubStruct3AggressiveInlining(MySubStruct3Dto source)
    {
        var target = new MySubStruct3();
        target.Int = source.Int;
        target.String = source.String;
        return target;
    }
    #endregion
    #endregion
}