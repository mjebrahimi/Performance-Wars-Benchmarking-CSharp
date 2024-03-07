
# Convert Enum To String Benchmark

## Key Results:

- `NetEscapades.EnumGenerators` nuget has the **Fastest** method for converting an enum to string, even faster than .Net built-in methods. Its secret is because it uses source generators.
- Using `nameof()` is the **Fastest** way to convert an enum to string among .Net built-in methods. As you see it takes 0.0 ns/op because it actually complies to a just literal string ([see the compiled code](https://sharplab.io/#v2:EYLgtghglgdgPgAQMwAIECYUGEUG8CwAUCiWqggIwAMKAsgBQCUeRpbaA7CjBGAKYB7AGb1aATwCiMAK5gAdAEEAMgJgBzBTAAmANT4AnYAIDOfKbIByvPowDcrUgF8HJF2RR8ZYOpK9uCxOwkyqoa2nqGJmZeVvxuzoSOQA))
- `EnumStringValues` nuget has the worst method for converting an enum to string. It does boxing/unboxing multiple times and also uses bunch of linq methods in background which IMO are the main reasons why it's so slow and inefficient.

Benchmarked NuGets:
- [NetEscapades.EnumGenerators](https://github.com/andrewlock/NetEscapades.EnumGenerators)
- [Supernova.Enum.Generators](https://www.nuget.org/packages/Supernova.Enum.Generators)
- [Enums.NET](https://www.nuget.org/packages/Enums.NET)
- [FastEnum](https://www.nuget.org/packages/FastEnum)
- [EnumUtils](https://www.nuget.org/packages/EnumUtils/2.1.8-pre)
- [EnumStringValues](https://www.nuget.org/packages/EnumStringValues)

![benchmark](Benchmark.png)