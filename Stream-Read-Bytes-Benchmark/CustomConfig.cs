using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Collections.Immutable;
using System.Reflection;

public class CustomConfig : ManualConfig
{
    public CustomConfig()
    {
        Orderer = new CustomOerderer();
    }

    private class CustomOerderer : IOrderer
    {
        public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase, IEnumerable<BenchmarkLogicalGroupRule> order = null)
            => benchmarksCase;

        public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary)
            => from benchmark in benchmarksCase
                orderby
                    Convert.ToInt32(benchmark.Parameters["FileSize"]) ascending,
                    benchmark.Parameters["FileStreamOption"].ToString(),
                    benchmark.Descriptor.WorkloadMethod.GetCustomAttribute<BenchmarkOrderAttribute>()?.Priority.ToString() ?? benchmark.Descriptor.WorkloadMethodDisplayInfo,
                    summary[benchmark]?.ResultStatistics?.Mean ?? 0
                select benchmark;

        public string GetHighlightGroupKey(BenchmarkCase benchmarkCase)
            => benchmarkCase.Parameters["FileSize"].ToString();

        public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase)
            => benchmarkCase.Parameters["FileSize"].ToString();

        public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule> order = null)
            => logicalGroups.OrderBy(it => it.Key);

        public bool SeparateLogicalGroups => true;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class BenchmarkOrderAttribute : PriorityAttribute
{
}