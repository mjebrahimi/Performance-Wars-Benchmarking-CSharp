using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Utilities;

public static class EnumerableGuard
{
    /// <summary>Throws an exception if <paramref name="argument"/> is null or empty.</summary>
    /// <param name="argument">The argument to validate as non-null and non-empty.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void ThrowIfNullOrEmpty<T>([NotNull] IEnumerable<T> argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(argument, paramName);

        //Performance Tip: using pattern matching on array is faster than TryGetNonEnumeratedCount
        if (argument is Array and { Length: 0 }
            || (argument.TryGetNonEnumeratedCount(out var count) && count == 0)
            || argument.Any() is false)
        {
            throw new ArgumentException("Argument is empty.", paramName);
        }
    }
}

public static class RangeGuard
{
    /// <summary>Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not in the range.</summary>
    /// <param name="value">The argument to validate.</param>
    /// <param name="from">The starting value to compare with <paramref name="value"/>.</param>
    /// <param name="to">The ending value to compare with <paramref name="value"/>.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ThrowIfNotInRange<T>(T value, T from, T to, bool inclusive = true, [CallerArgumentExpression(nameof(value))] string? paramName = null) where T : IComparable<T>
    {
        if (inclusive)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, from, paramName);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, to, paramName);
        }
        else
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, from, paramName);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, to, paramName);
        }
    }
}