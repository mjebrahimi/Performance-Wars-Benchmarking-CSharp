# Different methods to Reverse a string

## Key Results

**Fastest Methods (2x more allocation than string.Create) are 👇:**

```cs
public static string Reverse(string text)
{
    var charArray = text.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
}
//OR (same performance)
public static string Reverse(string text)
{
    Span<char> span = text.ToCharArray();
    span.Reverse();
    return new string(span); //span.ToString(); //the same
}
```

**Most CG efficient (a bit slower than previous) Method is 👇:**

```cs
public static string Reverse(string text)
{
    return string.Create(text.Length, text, (chars, state) =>
    {
        var pos = 0;
        for (int i = state.Length - 1; i >= 0; i--)
            chars[pos++] = state[i];
    });
}
```

![Benchmark](Benchmark.png)