``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.572 (2004/?/20H1)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.7.20366.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.36411, CoreFX 5.0.20.36411), X64 RyuJIT
  Job-VUCERT : .NET Core 5.0.0 (CoreCLR 5.0.20.36411, CoreFX 5.0.20.36411), X64 RyuJIT

RunStrategy=Throughput  

```
|                      Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |-----------:|----------:|----------:|-------:|--------:|-------:|------:|------:|----------:|
| &#39;10 times Ctor Instantiate&#39; | 417.246 ns | 6.6159 ns | 5.5246 ns | 120.54 |    1.75 | 0.6375 |     - |     - |    2000 B |
| &#39;10 times Lazy Instantiate&#39; |   3.458 ns | 0.0255 ns | 0.0226 ns |   1.00 |    0.00 |      - |     - |     - |         - |
