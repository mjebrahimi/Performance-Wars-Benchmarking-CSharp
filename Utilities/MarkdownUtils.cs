using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Utilities;

public static partial class MarkdownUtils
{
    private static readonly string[] separator = ["\r\n", "\r", "\n"];

    public static List<ExpandoObject> JoinMarkdownReports(IEnumerable<string> markdownTables, string[] equalityColumns, string pivotColumn, string commonColumn, bool colorize = false)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(markdownTables);
        EnumerableGuard.ThrowIfNullOrEmpty(equalityColumns);
        ArgumentException.ThrowIfNullOrWhiteSpace(pivotColumn);
        ArgumentException.ThrowIfNullOrWhiteSpace(commonColumn);

        var parsedTables = markdownTables.Select(ParseMarkdownTable).ToArray();
        if (colorize)
        {
            foreach (var list in parsedTables)
            {
                var splitedItems = list.SplitBy(IsNullOrAllPropsNullOrEmpty);
                foreach (var section in splitedItems)
                {
                    var statistics = section.Select(p =>
                    {
                        var str = p.GetProperty(commonColumn).ToString()!;
                        return ExtractNumber(str);
                    }).ToArray();

                    var min = statistics.Min();
                    var max = statistics.Max();
                    var diff = max - min;

                    foreach (var item in section)
                    {
                        var str = item.GetProperty(commonColumn).ToString()!;
                        var value = ExtractNumber(str);

                        var relativeScore = (max - value) / diff;
                        var color = ColorUtils.GetColorBetweenRedAndGreen(Convert.ToSingle(relativeScore));
                        color = ColorUtils.Lighten(color);

                        item.SetProperty("background-color", color);
                    }
                }
            }
        }

        var columnToKeep = new HashSet<string>(equalityColumns);
        var joinedList = parsedTables.Aggregate((current, next) =>
        {
            return current.Join(next,
                left =>
                {
                    var columnValues = equalityColumns.Select(propertyName => left?.GetProperty(propertyName)?.ToString()!.Trim('*') ?? "");
                    return string.Join('_', columnValues);
                },
                right =>
                {
                    var columnValues = equalityColumns.Select(propertyName => right.GetProperty(propertyName)?.ToString()!.Trim('*'));
                    return string.Join('_', columnValues);
                },
                (left, right) =>
                {
                    if (IsNullOrAllPropsNullOrEmpty(left))
                        return null;

                    var leftPropertyName = "";
                    try
                    {
                        leftPropertyName = left.GetProperty(pivotColumn).ToString()!.Trim('*');
                        left.ChangeProperty(commonColumn, leftPropertyName);
                    }
#pragma warning disable S108, S2486
                    catch { }
#pragma warning restore S108, S2486

                    var rightPropertyName = right.GetProperty(pivotColumn).ToString()!.Trim('*');
                    var rightPropertyValue = right.GetProperty(commonColumn);
                    left.SetProperty(rightPropertyName, rightPropertyValue);

                    foreach (var column in equalityColumns)
                    {
                        var value = left.GetProperty(column)?.ToString()!;
                        left.SetProperty(column, value?.Trim('*') ?? "");
                    }

                    var leftColorName = "";
                    try
                    {
                        leftColorName = $"{leftPropertyName}.background-color";
                        left.ChangeProperty("background-color", leftColorName);
                    }
#pragma warning disable S108, S2486
                    catch { }
#pragma warning restore S108, S2486

                    var rightColorName = $"{rightPropertyName}.background-color";
                    var rightColorValue = right.GetProperty("background-color");
                    left.SetProperty(rightColorName, rightColorValue);

                    columnToKeep.Add(leftPropertyName);
                    columnToKeep.Add(rightPropertyName);
                    columnToKeep.Add(leftColorName);
                    columnToKeep.Add(rightColorName);

                    return left;
                }).ToList()!;
        });

        foreach (var item in joinedList)
        {
            item.RemovePropertiesExcept(columnToKeep);
        }

        return joinedList;

        static bool IsNullOrAllPropsNullOrEmpty(ExpandoObject expandoObj)
        {
            if (expandoObj == null) return true;
            var expandoDict = expandoObj as IDictionary<string, object>;
            return expandoDict.Values.All(p => string.IsNullOrEmpty((string)p));
        }
    }

    public static void SaveAsMarkdownTable(this IEnumerable<ExpandoObject> source, string path)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(source);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var text = source.ToMarkdownTable();
        File.WriteAllText(path, text);
    }

    public static void SaveAsMarkdownTable<T>(this IEnumerable<T> source, string path)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(source);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var text = source.ToMarkdownTable();
        File.WriteAllText(path, text);
    }

    public static string ToMarkdownTable(this IEnumerable<ExpandoObject> source)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(source);

        var items = source.Select(p => (IDictionary<string, object>)p!);
        var columnNames = items.ElementAt(0)!.Keys;

        var maxColumnValues = items.Where(p => p is not null)
            .Select(x => columnNames.Select(p => x[p]?.ToString()?.Length ?? 0))
            .Union(new[] { columnNames.Select(p => p.Length) }) // Include header in column sizes
            .Aggregate(
                new int[columnNames.Count].AsEnumerable(),
                (accumulate, x) => accumulate.Zip(x, Math.Max))
            .ToArray();

        var headerLine = "| " + string.Join(" | ", columnNames.Select((n, i) => n.PadRight(maxColumnValues[i]))) + " |";

        var headerDataDividerLine = "| " + string.Join("| ", columnNames.Select((g, i) => new string('-', maxColumnValues[i]) + g.GetType().GetAlignChar())) + "|";

        var tableLines = items.Select(dic => "| " + string.Join(" | ", columnNames.Select((col, index) => (dic is null ? "" : dic[col]?.ToString() ?? "").PadRight(maxColumnValues[index]))) + " |");

        string[] lines = [
            headerLine,
            headerDataDividerLine,
            .. tableLines
        ];

        return string.Join(Environment.NewLine, lines);
    }

    public static string ToMarkdownTable<T>(this IEnumerable<T> source)
    {
        EnumerableGuard.ThrowIfNullOrEmpty(source);

        var properties = typeof(T).GetRuntimeProperties();
        var fields = typeof(T).GetRuntimeFields().Where(f => f.IsPublic);

        var gettables = Enumerable.Union(
                properties.Select(p => new { p.Name, GetValue = (Func<object?, object?>)p.GetValue, Type = p.PropertyType }),
                fields.Select(p => new { p.Name, GetValue = (Func<object?, object?>)p.GetValue, Type = p.FieldType })
            );

        var maxColumnValues = source
            .Select(x => gettables.Select(p => p.GetValue(x)?.ToString()?.Length ?? 0))
            .Union(new[] { gettables.Select(p => p.Name.Length) }) // Include header in column sizes
            .Aggregate(
                new int[gettables.Count()].AsEnumerable(),
                (accumulate, x) => accumulate.Zip(x, Math.Max))
            .ToArray();

        var columnNames = gettables.Select(p => p.Name);

        var headerLine = "| " + string.Join(" | ", columnNames.Select((n, i) => n.PadRight(maxColumnValues[i]))) + " |";

        var headerDataDividerLine = "| " + string.Join("| ", gettables.Select((g, i) => new string('-', maxColumnValues[i]) + g.Type.GetAlignChar())) + "|";

        var tableLines = source.Select(s => "| " + string.Join(" | ", gettables.Select((n, i) => (n.GetValue(s)?.ToString() ?? "").PadRight(maxColumnValues[i]))) + " |");

        string[] lines = [
            headerLine,
            headerDataDividerLine,
            .. tableLines
        ];

        return string.Join(Environment.NewLine, lines);
    }

    public static List<T> ParseMarkdownTable<T>(string markdownTable) where T : new()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(markdownTable);

        var objects = new List<T>();

        // Extract the table rows from the Markdown table
        var rows = markdownTable.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        if (rows.Length < 3)
        {
            // The table should have at least three rows (header, separator, and content)
            throw new ArgumentException("Invalid Markdown table format.");
        }

        // Extract the header row and determine the column names
        var headers = rows[0].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);

        // Extract the separator row to determine column widths
        var separators = rows[1].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);
        var columnWidths = new int[headers.Length];
        for (int i = 0; i < separators.Length; i++)
        {
            columnWidths[i] = separators[i].Trim().Length;
        }

        // Process the content rows
        for (int i = 2; i < rows.Length; i++)
        {
            var cells = rows[i].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);

            if (cells.Length != headers.Length)
            {
                // The number of cells in the row should match the number of columns
                throw new ArgumentException("Invalid Markdown table format.");
            }

            var obj = new T();

            for (int j = 0; j < headers.Length; j++)
            {
                var propertyName = headers[j].Trim();
                var cellValue = cells[j].Trim();

                var property = typeof(T).GetProperty(propertyName);
                if (property is not null)
                {
                    var value = Convert.ChangeType(cellValue, property.PropertyType);
                    // Use reflection to set the property value dynamically
                    property.SetValue(obj, value);
                }
            }

            objects.Add(obj);
        }

        return objects;
    }

    public static List<ExpandoObject> ParseMarkdownTable(string markdownTable)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(markdownTable);

        var objects = new List<ExpandoObject>();

        // Extract the table rows from the Markdown table
        var rows = markdownTable.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        if (rows.Length < 3)
        {
            // The table should have at least three rows (header, separator, and content)
            throw new ArgumentException("Invalid Markdown table format.");
        }

        // Extract the header row and determine the column names
        var headers = rows[0].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);

        // Extract the separator row to determine column widths
        var separators = rows[1].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);
        var columnWidths = new int[headers.Length];
        for (int i = 0; i < separators.Length; i++)
        {
            columnWidths[i] = separators[i].Trim().Length;
        }

        // Process the content rows
        for (int i = 2; i < rows.Length; i++)
        {
            var cells = rows[i].Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);

            if (cells.Length != headers.Length)
            {
                // The number of cells in the row should match the number of columns
                throw new ArgumentException("Invalid Markdown table format.");
            }

            var dynamicObj = new ExpandoObject();

            for (int j = 0; j < headers.Length; j++)
            {
                var propertyName = headers[j].Trim();
                var cellValue = cells[j].Trim();

                dynamicObj.SetProperty(propertyName, cellValue);
            }

            objects.Add(dynamicObj);
        }

        return objects;
    }

    private static char GetAlignChar(this Type type)
    {
        return IsNumericType(type) ? ':' : ' ';
    }

    private static bool IsNumericType(this Type type)
    {
        return type == typeof(byte) ||
               type == typeof(sbyte) ||
               type == typeof(ushort) ||
               type == typeof(uint) ||
               type == typeof(ulong) ||
               type == typeof(short) ||
               type == typeof(int) ||
               type == typeof(long) ||
               type == typeof(decimal) ||
               type == typeof(double) ||
               type == typeof(float);
    }

    private static decimal ExtractNumber(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var value = GetExtractNumberRegex().Match(input).Value;
        return decimal.Parse(value);
    }

    [GeneratedRegex(@"[-+]?\d{1,3}(,\d{3})*(\.\d+)?")]
    private static partial Regex GetExtractNumberRegex();
}
