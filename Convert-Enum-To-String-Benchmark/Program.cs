using BenchmarkDotNetVisualizer;
using Convert_Enum_To_String_Benchmark;

class Program
{
    public static async  Task Main(string[] args)
    {
#if DEBUG
        Console.ForegroundColor = System.ConsoleColor.Yellow;
        Console.WriteLine("*****To achieve accurate results, set project configuration to Release mode.*****");
        return;
#endif
        var summary = BenchmarkAutoRunner.Run<Benchmark>();

        await summary.SaveAsHtmlAndImageAsync(
            htmlPath: DirectoryHelper.GetPathRelativeToProjectDirectory("Reports\\VisualizedBenchmark.html"),
            imagePath: DirectoryHelper.GetPathRelativeToProjectDirectory("Reports\\VisualizedBenchmark.png"),
            options: new ReportHtmlOptions
            {
                Title = "Converting Enum To A String Benchmarks",
                GroupByColumns = ["Method"],
                SpectrumColumns = ["Mean", "Allocated"],
                DividerMode = RenderTableDividerMode.EmptyDividerRow,
                HtmlWrapMode = HtmlDocumentWrapMode.Simple
            });
    }
}