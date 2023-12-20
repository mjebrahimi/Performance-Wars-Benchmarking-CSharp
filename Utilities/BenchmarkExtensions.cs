using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Utilities;

public static class BenchmarkExtensions
{
    public static async Task JoinReportsAndSaveAsImageAsync(this Summary[] summaries, string title, string[] equalityColumns, string pivotColumn, string commonColumn, bool colorize = true, string fileName = "benchmark.png")
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var html = JoinReportsAsHtml(summaries, title, equalityColumns, pivotColumn, commonColumn, colorize);
        var imageBytes = await HtmlUtils.RenderHtmlToImageDataAsync(html);
        var compressedBytes = ImageCompressor.ImageSharp.CompressWebp(imageBytes);
        var resultsDir = summaries[0].ResultsDirectoryPath;
        var savePath = Path.Combine(resultsDir, "..", "..", "..", "..", "..", fileName);
        await File.WriteAllBytesAsync(savePath, compressedBytes);
    }

    public static Task JoinReportsAndSaveAsHtmlAsync(this Summary[] summaries, string title, string[] equalityColumns, string pivotColumn, string commonColumn, bool colorize = true, string fileName = "index.html")
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var html = JoinReportsAsHtml(summaries, title, equalityColumns, pivotColumn, commonColumn, colorize);
        var resultsDir = summaries[0].ResultsDirectoryPath;
        var savePath = Path.Combine(resultsDir, "..", "..", "..", "..", "..", fileName);
        return File.WriteAllTextAsync(savePath, html);
    }

    public static Task JoinReportsAndSaveAsMarkdownAsync(this Summary[] summaries, string title, string[] equalityColumns, string pivotColumn, string commonColumn, string fileName = "README.md")
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var markdown = JoinReportsAsMarkdown(summaries, title, equalityColumns, pivotColumn, commonColumn);
        var resultsDir = summaries[0].ResultsDirectoryPath;
        var savePath = Path.Combine(resultsDir, "..", "..", "..", "..", "..", fileName);
        return File.WriteAllTextAsync(savePath, markdown);
    }

    public static Task ConcatReportsAndSaveAsMarkdownAsync(this Summary[] summaries, string title, string fileName = "README.md")
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var markdown = ConcatReportsAsMarkdown(summaries, title);
        var resultsDir = summaries[0].ResultsDirectoryPath;
        var savePath = Path.Combine(resultsDir, "..", "..", "..", "..", "..", fileName);
        return File.WriteAllTextAsync(savePath, markdown);
    }

    public static string JoinReportsAsHtml(this Summary[] summaries, string title, string[] equalityColumns, string pivotColumn, string commonColumn, bool colorize = true)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        EnumerableGuard.ThrowIfNullOrEmpty(equalityColumns);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(pivotColumn);
        ArgumentException.ThrowIfNullOrWhiteSpace(commonColumn);

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(HtmlUtils.CSS);
        stringBuilder.AppendLine($"<h1>{title}</h1>");

        string? benchmarkInfo = null;
        var markdownReports = summaries.Select(summary =>
        {
            var benchmarkClassType = summary.BenchmarksCases[0].Descriptor.WorkloadMethod.ReflectedType!;

            var markdownText = summary.GetMarkdown();

            benchmarkInfo ??=
                $"""
                <pre>
                <code>
                {markdownText[..(markdownText.LastIndexOf("```") + 4)].Trim(' ', '\r', '\n', '`')}
                </code>
                </pre>
                """;

            markdownText = markdownText[(markdownText.LastIndexOf("```") + 4)..];

            return new { BenchmarkClassType = benchmarkClassType, MarkdownTableText = markdownText };
        });

        var groupedReports = markdownReports.GroupBy(p => p.BenchmarkClassType.GetCustomAttribute<DisplayAttribute>()!.GroupName, p => p.MarkdownTableText);
        foreach (var reports in groupedReports)
        {
            var joinedItems = MarkdownUtils.JoinMarkdownReports(reports, equalityColumns, pivotColumn, commonColumn, colorize);
            var html = HtmlUtils.ToHtmlTable(joinedItems, appendCSS: false);

            stringBuilder.AppendLine($"<h2>{reports.Key}</h2>")
                         .AppendLine(benchmarkInfo)
                         .AppendLine(html)
                         .AppendLine();
        }

        return stringBuilder.ToString();
    }

    public static string JoinReportsAsMarkdown(this Summary[] summaries, string title, string[] equalityColumns, string pivotColumn, string commonColumn)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        EnumerableGuard.ThrowIfNullOrEmpty(equalityColumns);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(pivotColumn);
        ArgumentException.ThrowIfNullOrWhiteSpace(commonColumn);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("# ")
                     .AppendLine(title)
                     .AppendLine();

        string? benchmarkInfo = null;
        var markdownReports = summaries.Select(summary =>
        {
            var benchmarkClassType = summary.BenchmarksCases[0].Descriptor.WorkloadMethod.ReflectedType!;

            var markdownText = summary.GetMarkdown();

            benchmarkInfo ??= markdownText[..(markdownText.LastIndexOf("```") + 4)];

            markdownText = markdownText[(markdownText.LastIndexOf("```") + 4)..];

            return new { BenchmarkClassType = benchmarkClassType, MarkdownTableText = markdownText };
        });

        var groupedReports = markdownReports.GroupBy(p => p.BenchmarkClassType.GetCustomAttribute<DisplayAttribute>()!.GroupName, p => p.MarkdownTableText);
        foreach (var reports in groupedReports)
        {
            var joinedItems = MarkdownUtils.JoinMarkdownReports(reports, equalityColumns, pivotColumn, commonColumn, true);
            var markdown = MarkdownUtils.ToMarkdownTable(joinedItems);

            stringBuilder.Append("## ")
                         .AppendLine(reports.Key)
                         .AppendLine()
                         .AppendLine(benchmarkInfo)
                         .AppendLine(markdown)
                         .AppendLine();
        }

        return stringBuilder.ToString();
    }

    public static string ConcatReportsAsMarkdown(this Summary[] summaries, string title)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(summaries);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("# ")
                     .AppendLine(title)
                     .AppendLine();

        var isFirst = true;
        foreach (var summary in summaries)
        {
            var benchmarkClassType = summary.BenchmarksCases[0].Descriptor.WorkloadMethod.ReflectedType!;

            var benchmarkClassName = benchmarkClassType.Name;

            var benchmarkDisplayName =
                benchmarkClassType.GetCustomAttribute<DisplayAttribute>()?.Name
                ?? benchmarkClassType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                ?? benchmarkClassName;

            var markdownText = summary.GetMarkdown();
            if (!isFirst)
            {
                //Remove first part of markdown text
                markdownText = markdownText[(markdownText.LastIndexOf("```") + 4)..];
            }

            stringBuilder.Append("## ")
                         .AppendLine(benchmarkDisplayName)
                         .AppendLine()
                         .AppendLine(markdownText)
                         .AppendLine();

            isFirst = false;
        }

        return stringBuilder.ToString();
    }

    public static string GetMarkdown(this Summary summary)
    {
        ArgumentNullException.ThrowIfNull(summary);

        using var writer = new StringWriter();
        using var logger = new TextLogger(writer);
        MarkdownExporter.GitHub.ExportToLog(summary, logger);
        return writer.ToString();
    }

    public static string GetHtml(this Summary summary)
    {
        ArgumentNullException.ThrowIfNull(summary);

        using var writer = new StringWriter();
        using var logger = new TextLogger(writer);
        HtmlExporter.Default.ExportToLog(summary, logger);
        return writer.ToString();
    }
}