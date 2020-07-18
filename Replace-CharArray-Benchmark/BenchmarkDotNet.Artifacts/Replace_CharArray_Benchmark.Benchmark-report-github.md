``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.959 (1909/November2018Update/19H2)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.3.20216.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
  Job-UAQQXC : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT

RunStrategy=Throughput  

```
|        Method |      Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------:|----------:|----------:|------:|--------:|-------:|------:|------:|----------:|
| ReplaceString | 944.63 ns | 14.738 ns | 13.065 ns | 11.46 |    0.16 | 0.2804 |     - |     - |     880 B |
|   ReplaceChar | 292.21 ns |  3.005 ns |  2.811 ns |  3.54 |    0.03 | 0.2804 |     - |     - |     880 B |
| StringBuilder | 257.94 ns |  1.763 ns |  1.649 ns |  3.13 |    0.03 | 0.0710 |     - |     - |     224 B |
|     CharArray | 105.89 ns |  0.946 ns |  0.838 ns |  1.28 |    0.01 | 0.0560 |     - |     - |     176 B |
|          Span |  82.53 ns |  0.526 ns |  0.439 ns |  1.00 |    0.00 | 0.0280 |     - |     - |      88 B |
