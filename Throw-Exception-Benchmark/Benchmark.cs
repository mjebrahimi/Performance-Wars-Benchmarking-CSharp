using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
namespace Throw_Exception_Benchmark
{
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    public class Benchmark
    {
        private readonly IDevideOperator _devideOperator;

        public Benchmark()
        {
            _devideOperator = new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideDecorator(new DevideOperator())))))))))))))));
        }

        [Benchmark(Description = "With Long StackTrace Exception")]
        public void With_Long_StackTrace_Exception()
        {
            try
            {
                _devideOperator.Devide(4, 0); // => throw DivideByZeroException
            }
            catch { }
        }

        [Benchmark(Baseline = true, Description = "Without Throw Exception")]
        public void Without_Throw_Exception()
        {
            _devideOperator.Devide(4, 2);
        }
    }
}
