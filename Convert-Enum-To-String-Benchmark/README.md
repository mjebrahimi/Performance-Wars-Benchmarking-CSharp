
# Convert Enum To String Benchmark

### Key Result:

- Using `nameof()` is the **Fastest** way to convert enum to a string.
- `nameof()` has the most efficient memory allocation beside `Enum.GetName()`(first overload).
- Using `Enum.GetName(Enum.GetName(typeof(MyEnum), MyEnum.ALongAndVerboseEnumName))` (second overload) has the worst speed.

![benchmark](Benchmark.png)