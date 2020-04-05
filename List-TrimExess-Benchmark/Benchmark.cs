using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System;
using System.Collections.Generic;

namespace List_TrimExess_Benchmark
{
    //[ShortRunJob]
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    public class Benchmark
    {
        //Since the List.Capacity increcment is as follows : 2, 4, 8, 16, ...., 32768 (equal to 2^15)
        //And 32768 (+1) is to increace the capacity to ‭65,536‬
        private const int _itemCount = 32769;

        [Benchmark(Description = "32769 Items With TrimExcess")]
        public void _10000_Items_With_TrimExcess()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < _itemCount; i++)
            {
                list.Add(Guid.NewGuid().ToString());
            }
            list.TrimExcess();
        }

        [Benchmark(Baseline = true, Description = "32769 Items Without TrimExcess")]
        public void _10000_Items_Without_TrimExcess()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < _itemCount; i++)
            {
                list.Add(Guid.NewGuid().ToString());
            }
        }
    }
}
