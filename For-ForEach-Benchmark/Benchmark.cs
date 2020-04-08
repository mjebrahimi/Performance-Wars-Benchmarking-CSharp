using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using System.Collections.Generic;
using System.Linq;

namespace For_ForEach_Benchmark
{
    //[ShortRunJob]
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    public class Benchmark
    {
        private List<int> Items;

        [GlobalSetup]
        public void Setup()
        {
            Items = Enumerable.Range(0, 10_000_000).ToList();
        }

        [Benchmark(Baseline = true, Description = "for")]
        public void For()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var _ = Items[i];
            }
        }

        [Benchmark(Description = "foreach")]
        public void Foreach()
        {
            foreach (var item in Items)
            {
                var _ = item;
            }
        }

        [Benchmark(Description = "ForEach()")]
        public void ForEachMethod()
        {
            Items.ForEach(p => { var _ = p; });
        }
    }
}
