using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Reflection_NonReflection_Benchmark
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*****To achieve accurate results, set project configuration to release mode.*****");
            return;
#endif

            BenchmarkRunner.Run<Tests>();
        }
    }

    [SimpleJob(RunStrategy.Throughput, invocationCount: 1_000_000)]
    public class Tests
    {
        [Benchmark]
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

        [Benchmark]
        public async ValueTask<int> FastCode()
        {
            int finalResult = 0;

            for (int i = 0; i < 100; i++)
            {
                var result1 = Sum2(1, 2);
                var result2 = await Sum2Async(1, 2);
                finalResult += result1.Sum + result2.Sum;
            }

            return finalResult;
        }

        public ResultClass Sum1(int n1, int n2)
        {
            return new ResultClass { Sum = n1 + n2 };
        }

        public async Task<ResultClass> Sum1Async(int n1, int n2)
        {
            return new ResultClass { Sum = n1 + n2 };
        }

        public ResultStruct Sum2(int n1, int n2)
        {
            return new ResultStruct { Sum = n1 + n2 };
        }

        public async ValueTask<ResultStruct> Sum2Async(int n1, int n2)
        {
            return new ResultStruct { Sum = n1 + n2 };
        }
    }

    public class ResultClass
    {
        public int Sum { get; set; }
    }

    public struct ResultStruct
    {
        public int Sum;
    }
}
