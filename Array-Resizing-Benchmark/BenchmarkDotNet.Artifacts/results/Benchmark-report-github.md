```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3085/23H2/2023Update/SunValley3)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  Job-ELPGOX : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

RunStrategy=Throughput  array=Byte[2000]  newSize=1000  

```
| Method                             | Kind     | Mean         | Error      | StdDev     | Allocated |
|----------------------------------- |--------- |-------------:|-----------:|-----------:|----------:|
| Array_Resize                       | Decrease |     2.083 ns |  0.0562 ns |  0.0526 ns |         - |
| Array_CollectionExpression         | Decrease |    43.583 ns |  0.8835 ns |  0.8264 ns |    1024 B |
| RuntimeHelpers_GetSubArray         | Decrease |    44.885 ns |  0.6604 ns |  0.5854 ns |    1024 B |
| Array_Copy                         | Decrease |    45.472 ns |  0.7345 ns |  0.6511 ns |    1024 B |
| Span_CollectionExpression_CopyTo   | Decrease |    46.310 ns |  0.6745 ns |  0.6309 ns |    1024 B |
| ArraySegment_ToArray               | Decrease |    46.774 ns |  0.7873 ns |  0.6980 ns |    1024 B |
| Span_Slice_ToArray                 | Decrease |    47.727 ns |  0.8822 ns |  1.0502 ns |    1024 B |
| Span_Slice_CopyTo                  | Decrease |    48.067 ns |  0.8410 ns |  0.8637 ns |    1024 B |
| Span_CollectionExpression_ToArray  | Decrease |    48.591 ns |  1.0134 ns |  0.9480 ns |    1024 B |
| Buffer_BlockCopy                   | Decrease |    48.646 ns |  0.9990 ns |  1.0689 ns |    1024 B |
| Array_CollectionExpression_ToArray | Decrease |   110.881 ns |  1.5986 ns |  3.9215 ns |    2048 B |
| ArraySegment_CollectionExpression  | Decrease |   899.541 ns |  6.8870 ns |  6.4421 ns |    1024 B |
| Linq_Take_ToArray                  | Decrease | 2,022.878 ns | 39.2665 ns | 43.6446 ns |    1072 B |
