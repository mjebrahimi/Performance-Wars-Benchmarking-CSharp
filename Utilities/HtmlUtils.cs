using PuppeteerSharp;
using System.Dynamic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities;

public static partial class HtmlUtils
{
    public const string CSS =
    """
    <style type="text/css">
        body { font-family: system-ui; }
        table { border-collapse: collapse; display: block; width: 100%; overflow: auto; }
        td, th { padding: 6px 13px; border: 1px solid #ddd; text-align: right; }
        tr { background-color: #fff; border-top: 1px solid #ccc; }
        tr:nth-child(even) { background: #f8f8f8; }
        pre { background: #f8f8f8; }
    </style>
    """;

    public static async Task RenderHtmlToImageAsync(string html, string path, string elementQuery = "html")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(html);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentException.ThrowIfNullOrWhiteSpace(elementQuery);

        using var browserFetcher = new BrowserFetcher();

        // Download chrome (headless) browser (first time takes a while).
        await browserFetcher.DownloadAsync();

        // Launch the browser and set the given html.
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        // Select the element and take a screen-shot
        await page.WaitForSelectorAsync(elementQuery, new WaitForSelectorOptions { Timeout = 2000 }); // Wait for the selector to load.
        var elementHandle = await page.QuerySelectorAsync(elementQuery); // Declare a variable with an ElementHandle.

        var options = new ScreenshotOptions
        {
            Type = ScreenshotType.Png,
            // Quality = Not supported with PNG images!
        };
        await elementHandle.ScreenshotAsync(path, options);

        await browser.CloseAsync();
    }

    public static async Task<byte[]> RenderHtmlToImageDataAsync(string html, string elementQuery = "html")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(html);
        ArgumentException.ThrowIfNullOrWhiteSpace(elementQuery);

        using var browserFetcher = new BrowserFetcher();

        // Download chrome (headless) browser (first time takes a while).
        await browserFetcher.DownloadAsync();

        // Launch the browser and set the given html.
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        // Select the element and take a screen-shot
        await page.WaitForSelectorAsync(elementQuery, new WaitForSelectorOptions { Timeout = 2000 }); // Wait for the selector to load.
        var elementHandle = await page.QuerySelectorAsync(elementQuery); // Declare a variable with an ElementHandle.

        var options = new ScreenshotOptions
        {
            Type = ScreenshotType.Png
            // Quality = Not supported with PNG images!
        };
        var bytes = await elementHandle.ScreenshotDataAsync(options);

        await browser.CloseAsync();
        return bytes;
    }

    public static void SaveAsHtmlTable(this IEnumerable<ExpandoObject> source, string path, bool appendCSS = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var text = source.ToHtmlTable(appendCSS);
        File.WriteAllText(path, text);
    }

    public static void SaveAsHtmlTable<T>(this IEnumerable<T> source, string path, bool appendCSS = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var text = source.ToHtmlTable(appendCSS);
        File.WriteAllText(path, text);
    }

    public static string ToHtmlTable(this IEnumerable<ExpandoObject> source, bool appendCSS = true)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(source);

        var items = source.Select(p => (IDictionary<string, object>)p!);
        var props = items.ElementAt(0)!.Keys.Where(p => !p.EndsWith("background-color")).ToList();

        var html = new StringBuilder();

        if (appendCSS)
            html.AppendLine(CSS);

        html.AppendLine("<table>");

        //Header
        html.AppendLine("<thead>")
            .AppendLine("<tr>");
        foreach (var prop in props)
        {
            html.Append("<th>").Append(prop).AppendLine("</th>");
        }
        html.AppendLine("</tr>")
            .AppendLine("</thead>");

        //Body
        html.AppendLine("<tbody>");
        foreach (var item in source)
        {
            var dictionary = (IDictionary<string, object>)item!;
            html.AppendLine("<tr>");

            if (dictionary is null)
            {
                html.Append($"<td colspan=\"{props.Count}\">").AppendLine("</td>");
                continue;
            }

            foreach (var prop in props)
            {
                var value = dictionary[prop]?.ToString()?.ReplaceMarkdownBoldWithHtmlStrong() ?? "";
                var style = dictionary.TryGetValue($"{prop}.background-color", out var color) ? $" style=\"background: {color};\"" : "";

                html.Append($"<td{style}>").Append(value).AppendLine("</td>");
            }

            html.AppendLine("</tr>");
        }

        html.AppendLine("</tbody>")
            .AppendLine("</table>");

        return html.ToString();
    }

    public static string ToHtmlTable<T>(this IEnumerable<T> source, bool appendCSS = true)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(source);

        var props = typeof(T).GetProperties();

        var html = new StringBuilder();

        if (appendCSS)
            html.AppendLine(CSS);

        html.AppendLine("<table>");

        //Header
        html.AppendLine("<thead>")
            .AppendLine("<tr>");
        foreach (var prop in props)
        {
            html.Append("<th>").Append(prop.Name).AppendLine("</th>");
        }
        html.AppendLine("</tr>")
            .AppendLine("</thead>");

        //Body
        html.AppendLine("<tbody>");
        foreach (var item in source)
        {
            html.AppendLine("<tr>");
            if (item is null)
            {
                html.Append($"<td colspan=\"{props.Length}\">").AppendLine("</td>");
                continue;
            }
            foreach (var prop in props)
            {
                var value = prop.GetValue(item)?.ToString() ?? "";
                html.Append("<td>").Append(value).AppendLine("</td>");
            }
            html.AppendLine("</tr>");
        }

        html.AppendLine("</tbody>")
            .AppendLine("</table>");

        return html.ToString();
    }

    public static string ReplaceMarkdownBoldWithHtmlStrong(this string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return GetMarkdownBoldRegex().Replace(text, "<strong>$1</strong>"); //"<b>$1</b>"
    }

    [GeneratedRegex(@"\*\*(.*?)\*\*")]
    private static partial Regex GetMarkdownBoldRegex();
}