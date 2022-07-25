using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using Prometheus;

namespace Metrics_Benchmark
{
    //[DryJob]
    //[ShortRunJob]
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    public class BenchmarkContainer
    {
        private static Counter counter = Metrics.CreateCounter("counter", "help");
        private static Gauge gauge = Metrics.CreateGauge("gauge", "help");
        private static Histogram histogram = Metrics.CreateHistogram("histogram", "help", new HistogramConfiguration
        {
            Buckets = Prometheus.Histogram.ExponentialBuckets(0.001, 2, 16), // 1 ms to 32K ms buckets (start from 0.001 sec and multiple by 2 for 16 times)
        });

        [Benchmark]
        public void Counter()
        {
            counter.Inc();
        }

        [Benchmark]
        public void Gauge()
        {
            gauge.Set(128);
        }

        [Benchmark]
        public void Histogram()
        {
            histogram.Observe(7);
        }
    }
}
