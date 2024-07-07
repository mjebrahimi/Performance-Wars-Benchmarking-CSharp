using Models;
using System.Runtime.CompilerServices;

public static class Mapper
{
    #region Class
    public static MyClass MapSimplified_NoAggressiveInlining(MyClassDto source)
    {
        return new MyClass
        {
            AlbumType = source.AlbumType,
            Href = source.Href,
            Id = source.Id,
            Name = source.Name,
            Popularity = source.Popularity,
            ReleaseDate = source.ReleaseDate,
            ReleaseDatePrecision = source.ReleaseDatePrecision,
            Track = new MySubClass
            {
                Href = source.Track.Href,
                Limit = source.Track.Limit,
                Offset = source.Track.Offset,
                Total = source.Track.Total
            },
            Type = source.Type,
            Uri = source.Uri
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyClass MapSimplified_AggressiveInlining(MyClassDto source)
    {
        return new MyClass
        {
            AlbumType = source.AlbumType,
            Href = source.Href,
            Id = source.Id,
            Name = source.Name,
            Popularity = source.Popularity,
            ReleaseDate = source.ReleaseDate,
            ReleaseDatePrecision = source.ReleaseDatePrecision,
            Track = new MySubClass
            {
                Href = source.Track.Href,
                Limit = source.Track.Limit,
                Offset = source.Track.Offset,
                Total = source.Track.Total
            },
            Type = source.Type,
            Uri = source.Uri
        };
    }

    public static MyClass Map_NoAggressiveInlining(MyClassDto source)
    {
        var target = new MyClass();
        target.AlbumType = source.AlbumType;
        target.Href = source.Href;
        target.Id = source.Id;
        target.Name = source.Name;
        target.Popularity = source.Popularity;
        target.ReleaseDate = source.ReleaseDate;
        target.ReleaseDatePrecision = source.ReleaseDatePrecision;
        target.Track = MapToClassTracks_NoAggressiveInlining(source.Track);
        target.Type = source.Type;
        target.Uri = source.Uri;
        return target;
    }

    private static MySubClass MapToClassTracks_NoAggressiveInlining(MySubClassDto source)
    {
        var target = new MySubClass();
        target.Href = source.Href;
        target.Limit = source.Limit;
        target.Offset = source.Offset;
        target.Total = source.Total;
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyClass Map_AggressiveInlining(MyClassDto source)
    {
        var target = new MyClass();
        target.AlbumType = source.AlbumType;
        target.Href = source.Href;
        target.Id = source.Id;
        target.Name = source.Name;
        target.Popularity = source.Popularity;
        target.ReleaseDate = source.ReleaseDate;
        target.ReleaseDatePrecision = source.ReleaseDatePrecision;
        target.Track = MapToClassTracks_AggressiveInlining(source.Track);
        target.Type = source.Type;
        target.Uri = source.Uri;
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubClass MapToClassTracks_AggressiveInlining(MySubClassDto source)
    {
        var target = new MySubClass();
        target.Href = source.Href;
        target.Limit = source.Limit;
        target.Offset = source.Offset;
        target.Total = source.Total;
        return target;
    }
    #endregion

    #region Struct
    public static MyStruct MapSimplified_NoAggressiveInlining(MyStructDto source)
    {
        return new MyStruct
        {
            AlbumType = source.AlbumType,
            Href = source.Href,
            Id = source.Id,
            Name = source.Name,
            Popularity = source.Popularity,
            ReleaseDate = source.ReleaseDate,
            ReleaseDatePrecision = source.ReleaseDatePrecision,
            Track = new MySubStruct
            {
                Href = source.Track.Href,
                Limit = source.Track.Limit,
                Offset = source.Track.Offset,
                Total = source.Track.Total
            },
            Type = source.Type,
            Uri = source.Uri
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyStruct MapSimplified_AggressiveInlining(MyStructDto source)
    {
        return new MyStruct
        {
            AlbumType = source.AlbumType,
            Href = source.Href,
            Id = source.Id,
            Name = source.Name,
            Popularity = source.Popularity,
            ReleaseDate = source.ReleaseDate,
            ReleaseDatePrecision = source.ReleaseDatePrecision,
            Track = new MySubStruct
            {
                Href = source.Track.Href,
                Limit = source.Track.Limit,
                Offset = source.Track.Offset,
                Total = source.Track.Total
            },
            Type = source.Type,
            Uri = source.Uri
        };
    }

    public static MyStruct Map_NoAggressiveInlining(MyStructDto source)
    {
        var target = new MyStruct();
        target.AlbumType = source.AlbumType;
        target.Href = source.Href;
        target.Id = source.Id;
        target.Name = source.Name;
        target.Popularity = source.Popularity;
        target.ReleaseDate = source.ReleaseDate;
        target.ReleaseDatePrecision = source.ReleaseDatePrecision;
        target.Track = MapToStructTracks_NoAggressiveInlining(source.Track);
        target.Type = source.Type;
        target.Uri = source.Uri;
        return target;
    }

    private static MySubStruct MapToStructTracks_NoAggressiveInlining(MySubStructDto source)
    {
        var target = new MySubStruct();
        target.Href = source.Href;
        target.Limit = source.Limit;
        target.Offset = source.Offset;
        target.Total = source.Total;
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MyStruct Map_AggressiveInlining(MyStructDto source)
    {
        var target = new MyStruct();
        target.AlbumType = source.AlbumType;
        target.Href = source.Href;
        target.Id = source.Id;
        target.Name = source.Name;
        target.Popularity = source.Popularity;
        target.ReleaseDate = source.ReleaseDate;
        target.ReleaseDatePrecision = source.ReleaseDatePrecision;
        target.Track = MapToStructTracks_AggressiveInlining(source.Track);
        target.Type = source.Type;
        target.Uri = source.Uri;
        return target;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static MySubStruct MapToStructTracks_AggressiveInlining(MySubStructDto source)
    {
        var target = new MySubStruct();
        target.Href = source.Href;
        target.Limit = source.Limit;
        target.Offset = source.Offset;
        target.Total = source.Total;
        return target;
    }
    #endregion

    #region Comment
    //#region Class
    //public static MyClass MapSimplified(MyClassDto source)
    //{
    //    return new MyClass()
    //    {
    //        Int = source.Int,
    //        Boolean = source.Boolean,
    //        DateTime = source.DateTime,
    //        Decimal = source.Decimal,
    //        Double = source.Double,
    //        Enum = source.Enum,
    //        Long = source.Long,
    //        String = source.String,
    //        SubClass = new MySubClass
    //        {
    //            Int = source.SubClass.Int,
    //            String = source.SubClass.String
    //        }
    //    };
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static MyClass MapSimplifiedAggressiveInlining(MyClassDto source)
    //{
    //    return new MyClass()
    //    {
    //        Int = source.Int,
    //        Boolean = source.Boolean,
    //        DateTime = source.DateTime,
    //        Decimal = source.Decimal,
    //        Double = source.Double,
    //        Enum = source.Enum,
    //        Long = source.Long,
    //        String = source.String,
    //        SubClass = new MySubClass
    //        {
    //            Int = source.SubClass.Int,
    //            String = source.SubClass.String
    //        }
    //    };
    //}

    //public static MyClass MapNoAggressiveInlining(MyClassDto source)
    //{
    //    var target = new MyClass();
    //    target.Int = source.Int;
    //    target.Boolean = source.Boolean;
    //    target.DateTime = source.DateTime;
    //    target.Decimal = source.Decimal;
    //    target.Double = source.Double;
    //    target.Enum = source.Enum;
    //    target.Long = source.Long;
    //    target.String = source.String;
    //    target.SubClass = MapSubClassNoAggressiveInlining(source.SubClass);
    //    return target;
    //}

    //private static MySubClass MapSubClassNoAggressiveInlining(MySubClassDto source)
    //{
    //    var target = new MySubClass();
    //    target.Int = source.Int;
    //    target.String = source.String;
    //    return target;
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static MyClass MapAggressiveInlining(MyClassDto source)
    //{
    //    var target = new MyClass();
    //    target.Int = source.Int;
    //    target.Boolean = source.Boolean;
    //    target.DateTime = source.DateTime;
    //    target.Decimal = source.Decimal;
    //    target.Double = source.Double;
    //    target.Enum = source.Enum;
    //    target.Long = source.Long;
    //    target.String = source.String;
    //    target.SubClass = MapSubClassAggressiveInlining(source.SubClass);
    //    return target;
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private static MySubClass MapSubClassAggressiveInlining(MySubClassDto source)
    //{
    //    var target = new MySubClass();
    //    target.Int = source.Int;
    //    target.String = source.String;
    //    return target;
    //}
    //#endregion

    //#region Struct
    //public static MyStruct MapSimplified(MyStructDto source)
    //{
    //    return new MyStruct()
    //    {
    //        Int = source.Int,
    //        Boolean = source.Boolean,
    //        DateTime = source.DateTime,
    //        Decimal = source.Decimal,
    //        Double = source.Double,
    //        Enum = source.Enum,
    //        Long = source.Long,
    //        String = source.String,
    //        SubStruct = new MySubStruct
    //        {
    //            Int = source.SubStruct.Int,
    //            String = source.SubStruct.String
    //        }
    //    };
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static MyStruct MapSimplifiedAggressiveInlining(MyStructDto source)
    //{
    //    return new MyStruct()
    //    {
    //        Int = source.Int,
    //        Boolean = source.Boolean,
    //        DateTime = source.DateTime,
    //        Decimal = source.Decimal,
    //        Double = source.Double,
    //        Enum = source.Enum,
    //        Long = source.Long,
    //        String = source.String,
    //        SubStruct = new MySubStruct
    //        {
    //            Int = source.SubStruct.Int,
    //            String = source.SubStruct.String
    //        }
    //    };
    //}

    //public static MyStruct MapNoAggressiveInlining(MyStructDto source)
    //{
    //    var target = new MyStruct();
    //    target.Int = source.Int;
    //    target.Boolean = source.Boolean;
    //    target.DateTime = source.DateTime;
    //    target.Decimal = source.Decimal;
    //    target.Double = source.Double;
    //    target.Enum = source.Enum;
    //    target.Long = source.Long;
    //    target.String = source.String;
    //    target.SubStruct = MapSubStructNoAggressiveInlining(source.SubStruct);
    //    return target;
    //}

    //private static MySubStruct MapSubStructNoAggressiveInlining(MySubStructDto source)
    //{
    //    var target = new MySubStruct();
    //    target.Int = source.Int;
    //    target.String = source.String;
    //    return target;
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public static MyStruct MapAggressiveInlining(MyStructDto source)
    //{
    //    var target = new MyStruct();
    //    target.Int = source.Int;
    //    target.Boolean = source.Boolean;
    //    target.DateTime = source.DateTime;
    //    target.Decimal = source.Decimal;
    //    target.Double = source.Double;
    //    target.Enum = source.Enum;
    //    target.Long = source.Long;
    //    target.String = source.String;
    //    target.SubStruct = MapSubStructAggressiveInlining(source.SubStruct);
    //    return target;
    //}

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //private static MySubStruct MapSubStructAggressiveInlining(MySubStructDto source)
    //{
    //    var target = new MySubStruct();
    //    target.Int = source.Int;
    //    target.String = source.String;
    //    return target;
    //}
    //#endregion
    #endregion
}