using BenchmarkDotNetVisualizer;
using System.ComponentModel;
using System.Reflection;

var summaries = BenchmarkAutoRunner.SwitcherRun(typeof(Program).Assembly);

Dictionary<Type, string> dictionary = new()
{
    [typeof(StaticBenchmark)] = "Benchmark-Static.png",
    [typeof(InstanceBenchmark)] = "Benchmark-Instance.png",
    [typeof(UnsafeAccessorBenchmark)] = "Benchmark-UnsafeAccessor.png",
};

foreach (var summary in summaries)
{
    var benchmarkType = summary.BenchmarksCases[0].Descriptor.Type;
    var title = benchmarkType.GetCustomAttribute<DisplayNameAttribute>()!.DisplayName;
    var fileName = dictionary[benchmarkType];

    await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory(fileName),
    options: new ReportHtmlOptions
    {
        Title = title,
        GroupByColumns = ["Categories"],
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
    });
}

Console.ReadLine();