``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.572 (2004/?/20H1)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.7.20366.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.36411, CoreFX 5.0.20.36411), X64 RyuJIT
  Job-VNBHYB : .NET Core 5.0.0 (CoreCLR 5.0.20.36411, CoreFX 5.0.20.36411), X64 RyuJIT

RunStrategy=Throughput  

```
|                    Method |        Mean |     Error |    StdDev |  Ratio | RatioSD |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------------- |------------:|----------:|----------:|-------:|--------:|----------:|------:|------:|-----------:|
| &#39;100000 Ctor Instantiate&#39; | 4,212.61 μs | 46.483 μs | 43.480 μs | 119.49 |    1.94 | 6375.0000 |     - |     - | 20000000 B |
| &#39;100000 Lazy Instantiate&#39; |    35.26 μs |  0.343 μs |  0.321 μs |   1.00 |    0.00 |         - |     - |     - |          - |
