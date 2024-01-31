using BenchmarkDotNetVisualizer;

#region Create form Artifacts result
var benchmarkInfo = BenchmarkInfo.CreateFromFile(Path.Combine(DirectoryHelper.GetProjectBenchmarkArtifactResultsDirectory(), "Benchmark-report-github.md"));
await VisualizeAsync(benchmarkInfo);
return;
#endregion

var summary = BenchmarkAutoRunner.Run<Benchmark>();

DirectoryHelper.MoveBenchmarkArtifactsToProjectDirectory();

await VisualizeAsync(summary.GetBenchmarkInfo());

Console.ReadLine();

async Task VisualizeAsync(BenchmarkInfo benchmarkInfo)
{
    var htmlPath = DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.html");
    var imagePath = DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png");
    var options = new ReportHtmlOptions
    {
        Title = "Different ways to Resize an Array",
        HighlightGroups = false,
        SortByColumns = ["Mean"],
        SpectrumColumns = ["Mean"],
        DividerMode = RenderTableDividerMode.SeparateTables,
        HtmlWrapMode = HtmlDocumentWrapMode.RichDataTables
    };
    await benchmarkInfo.SaveAsHtmlAndImageAsync(htmlPath, imagePath, options);

}