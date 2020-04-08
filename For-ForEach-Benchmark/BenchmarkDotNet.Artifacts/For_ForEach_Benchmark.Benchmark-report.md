``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.2.20176.6
  [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  Job-YUXMPN : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

RunStrategy=Throughput  

```
|    Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------- |---------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
|       for | 12.73 ms | 0.169 ms | 0.149 ms |  1.00 |    0.00 |     - |     - |     - |      76 B |
|   foreach | 25.40 ms | 0.555 ms | 1.556 ms |  2.13 |    0.08 |     - |     - |     - |         - |
| ForEach() | 24.71 ms | 0.510 ms | 0.867 ms |  1.94 |    0.06 |     - |     - |     - |         - |
