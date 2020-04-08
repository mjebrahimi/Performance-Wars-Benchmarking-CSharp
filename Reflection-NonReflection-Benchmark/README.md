# Reflection/Class vs NonReflection/Struct Benchmark

We've a slow code which is known to be very slow! We're going to benchmark it to find the fact!

```cs
public async Task<int> SlowCode()
{
     int finalResult = 0;

     for (int i = 0; i < 100; i++)
     {
          var result1 = (ResultClass)typeof(Tests).GetMethod("Sum1").Invoke(this, new object[] { 1, 2 });
          var result2 = await ((Task<ResultClass>)typeof(Tests).GetMethod("Sum1Async").Invoke(this, new object[] { 1, 2 }));
          finalResult += result1.Sum + result2.Sum;
     }

     return finalResult;
}
```

[SlowCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L28-L40) has reflection, uses Task instead of ValueTask. It also suffers from boxing and uses class intead of struct.

I'm going to compare its performance with [FastCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L43-L55) which uses struct instead of class, it uses direct method call instead of reflection and it uses ValueTask instead of Task. It also uses struct instead of class.

SlowCode performs reflection and other bad codes 100 times, and every 100 iterations only took 48.459 us!

``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.503 (1809/October2018Update/Redstone5)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 3.0.0-preview5-27626-15 (CoreCLR 4.6.27622.75, CoreFX 4.700.19.22408), 64bit RyuJIT
  Job-AYLPYT : .NET Core 3.0.0-preview5-27626-15 (CoreCLR 4.6.27622.75, CoreFX 4.700.19.22408), 64bit RyuJIT

InvocationCount=1000000  RunStrategy=Throughput  

```

|   Method |      Mean |     Error |    StdDev |
|--------- |----------:|----------:|----------:|
| SlowCode | 48.459 us | 0.0456 us | 0.0381 us |
| FastCode |  2.247 us | 0.0023 us | 0.0021 us |

Thanks to [Yaser Moradi](https://github.com/ysmoradi)