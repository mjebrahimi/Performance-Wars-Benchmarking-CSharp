``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.900 (1909/November2018Update/19H2)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.3.20216.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
  Job-PSBXEW : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT

RunStrategy=Throughput  

```
|                           Method |         Mean |      Error |     StdDev |  Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------------------- |-------------:|-----------:|-----------:|-------:|--------:|-------:|------:|------:|----------:|
| &#39;With Long StackTrace Exception&#39; | 29,522.01 ns | 402.728 ns | 336.296 ns | 798.80 |   13.71 | 0.0916 |     - |     - |     320 B |
|        &#39;Without Throw Exception&#39; |     36.97 ns |   0.449 ns |   0.420 ns |   1.00 |    0.00 |      - |     - |     - |         - |
