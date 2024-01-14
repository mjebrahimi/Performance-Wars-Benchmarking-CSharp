namespace Utilities;

public static class StringExtensions
{
    public static bool StartsWithSpan(this string str, ReadOnlySpan<char> value)
    {
        return str.AsSpan().StartsWith(value);
    }

    public static string Reverse(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return str;

        //A bit faster but allocates 2x more memory
        //Span<char> span = str.ToCharArray();
        //span.Reverse();
        //return new string(span);

        return string.Create(str.Length, str, (chars, state) =>
        {
            var pos = 0;
            for (int i = state.Length - 1; i >= 0; i--)
                chars[pos++] = state[i];
        });
    }

    public static string En2FaNumber(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return str;

        //A bit faster but allocates 2x more memory
        //Span<char> span = str.ToCharArray();
        //for (int i = 0; i < span.Length; i++)
        //{
        //    span[i] = (span[i]) switch
        //    {
        //        '0' => '\u06F0',
        //        '1' => '\u06F1',
        //        '2' => '\u06F2',
        //        '3' => '\u06F3',
        //        '4' => '\u06F4',
        //        '5' => '\u06F5',
        //        '6' => '\u06F6',
        //        '7' => '\u06F7',
        //        '8' => '\u06F8',
        //        '9' => '\u06F9',
        //        _ => span[i],
        //    };
        //}
        //return new string(span);

        return string.Create(str.Length, str, (chars, context) =>
        {
            for (int i = 0; i < context.Length; i++)
            {
                chars[i] = (context[i]) switch
                {
                    '0' => '\u06F0',
                    '1' => '\u06F1',
                    '2' => '\u06F2',
                    '3' => '\u06F3',
                    '4' => '\u06F4',
                    '5' => '\u06F5',
                    '6' => '\u06F6',
                    '7' => '\u06F7',
                    '8' => '\u06F8',
                    '9' => '\u06F9',
                    _ => context[i],
                };
            }
        });
    }
}