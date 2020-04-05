``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.2.20176.6
  [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  Job-OYPDSX : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

RunStrategy=Throughput  

```
|                           Method |     Mean |    Error |   StdDev | Ratio | RatioSD |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|--------------------------------- |---------:|---------:|---------:|------:|--------:|---------:|---------:|---------:|----------:|
|    &#39;32769 Items With TrimExcess&#39; | 11.85 ms | 0.240 ms | 0.703 ms |  1.16 |    0.07 | 671.8750 | 343.7500 | 328.1250 |   4.25 MB |
| &#39;32769 Items Without TrimExcess&#39; | 10.56 ms | 0.201 ms | 0.188 ms |  1.00 |    0.00 | 578.1250 | 296.8750 | 281.2500 |      4 MB |
