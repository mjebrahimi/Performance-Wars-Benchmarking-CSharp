/// <summary>
/// An example of implementing IEquatable<T>, IComparable<T>, IComparable, ==, !=, >, <, >=, <=
/// </summary>
public class MyClass :
    IEquatable<MyClass>,
    IComparable<MyClass>, IComparable
{
    public int Prop1 { get; set; }
    public int Prop2 { get; set; }
    public int Prop3 { get; set; }

    public override int GetHashCode()
    {
        // unchecked only needed if you're compiling with arithmetic checks enabled
        // (the default compiler behaviour is *disabled*, so most folks won't need this)
        unchecked // Overflow is fine, just wrap
        {
            var hash = 13 /*17*/;
            hash = (hash * 7 /*23*/) + Prop1.GetHashCode();
            hash = (hash * 7 /*23*/) + Prop2.GetHashCode();
            hash = (hash * 7 /*23*/) + Prop3.GetHashCode();
            return hash;
        }
        //return HashCode.Combine(Prop1, Prop2, Prop2); //is slower
    }

    public override bool Equals(object obj)
    {
        if (obj is not MyClass other)
            return false;

        return Equals(other);
    }

    public bool Equals(MyClass other)
    {
        if (other is null)
            return false;

        return Prop1 == other.Prop1 &&
               Prop2 == other.Prop2 &&
               Prop3 == other.Prop3;
    }

    public int CompareTo(MyClass other)
    {
        if (other is null)
            return 1;

        var result = Prop1.CompareTo(other.Prop1);
        if (result == 0)
        {
            result = Prop2.CompareTo(other.Prop2);
            if (result == 0)
                result = Prop3.CompareTo(other.Prop3);
        }
        return result;
    }

    public int CompareTo(object obj)
    {
        if (obj is null)
            return 1;

        if (obj is MyClass other)
            return CompareTo(other);

        throw new ArgumentException($"Argument obj with type {obj.GetType()} is invalid for Compare.", nameof(obj));
    }

    public static bool operator ==(MyClass left, MyClass right)
    {
        return left is null ? right is null : left.Equals(right);
        //return EqualityComparer<Person>.Default.Equals(left, right); //is slower
    }

    public static bool operator !=(MyClass left, MyClass right)
    {
        return !(left == right);
    }

    public static bool operator <(MyClass left, MyClass right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator <=(MyClass left, MyClass right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(MyClass left, MyClass right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator >=(MyClass left, MyClass right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}