using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Collections.Immutable;

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

        public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCases, Summary summary)
            => from benchmarkCase in benchmarksCases
               orderby
                   benchmarkCase.Parameters["Kind"].ToString(),
                   string.Join('_', benchmarkCase.Descriptor.Categories),
                   summary[benchmarkCase]?.ResultStatistics?.Mean ?? 0
               select benchmarkCase;

        public string GetHighlightGroupKey(BenchmarkCase benchmarkCase)
            => string.Join('_', benchmarkCase.Parameters["Kind"], string.Join('_', benchmarkCase.Descriptor.Categories));

        public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase)
            => string.Join('_', benchmarkCase.Parameters["Kind"], string.Join('_', benchmarkCase.Descriptor.Categories));

        public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule> order = null)
            => logicalGroups.OrderBy(it => it.Key);

        public bool SeparateLogicalGroups => true;
    }
}