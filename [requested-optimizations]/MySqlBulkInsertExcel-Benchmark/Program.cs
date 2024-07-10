using BenchmarkDotNetVisualizer;
using MySqlBulkInsertExcel_Benchmark;

var summary = BenchmarkAutoRunner.Run<Benchmark>();
await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "MySQL Bulk Insert Benchmark",
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = false
    });

Console.ReadLine();