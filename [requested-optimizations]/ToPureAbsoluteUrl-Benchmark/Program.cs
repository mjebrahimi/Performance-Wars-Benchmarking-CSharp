using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNetVisualizer;
using System.Globalization;
using System.Text;

#region Tests
Dictionary<string, string> urls = new()
{
    {"https://sub.domain.com/segment1/segment2/resource-common-core/-/endpoint_name/216?q1=01&q2=02", "segment1/segment2/resource-common-core/-/endpoint_name"},
    {"https://sub.domain.com/segment1/segment2/resource-common-core/-/endpoint_name/216/", "segment1/segment2/resource-common-core/-/endpoint_name"},
    {"https://sub.domain.com/segment1/segment2/resource-common-core/-/endpoint_name/216", "segment1/segment2/resource-common-core/-/endpoint_name"},
    {"https://sub.domain.com/segment/api/v2/resources/6037/endpoint/", "segment/api/v2/resources/endpoint"},
    {"https://sub.domain.com/segment/api/v2/resources/6037/endpoint", "segment/api/v2/resources/endpoint"},
    {"https://sub.domain.com/ResourceName/EndpointName?q1=1129&q2=001", "ResourceName/EndpointName"},
    {"/ResourceName/EndpointName?q1=1129&q2=001", "ResourceName/EndpointName"},
    {"/ResourceName/EndpointName?q1=1129&q2=001/", "ResourceName/EndpointName"},
    {"ResourceName/EndpointName?q1=1129&q2=001", "ResourceName/EndpointName"},
    {"ResourceName/EndpointName?q1=1129&q2=001/", "ResourceName/EndpointName"},
    {"ResourceName/EndpointName", "ResourceName/EndpointName"},
    {"/ResourceName/EndpointName", "ResourceName/EndpointName"},
    {"/ResourceName/EndpointName/", "ResourceName/EndpointName"},
    {"/ResourceName/EndpointName/216?q1=01&q2=02", "ResourceName/EndpointName/"}, //Methods can not handle this url
};

foreach (var (url, expected) in urls)
{
    var uri = new Uri(url, UriKind.RelativeOrAbsolute);

    var result0 = uri.ToPureAbsoluteUrl();
    if (result0 != expected)
    {
        Console.WriteLine($"Failed - ToPureAbsoluteUrl            - {url}");
    }

    var result1 = uri.ToPureAbsoluteUrl_Optimized1();
    if (result1 != expected)
    {
        Console.WriteLine($"Failed - ToPureAbsoluteUrl_Optimized1 - {url}");
    }

    var result2 = uri.ToPureAbsoluteUrl_Optimized2();
    if (result2 != expected)
    {
        Console.WriteLine($"Failed - ToPureAbsoluteUrl_Optimized2 - {url}");
    }

    var result3 = uri.ToPureAbsoluteUrl_Optimized3();
    if (result3 != expected)
    {
        Console.WriteLine($"Failed - ToPureAbsoluteUrl_Optimized3 - {url}");
    }
}
#endregion

var summary = BenchmarkAutoRunner.Run<Benchmark>();

await summary.SaveAsImageAsync(
    path: DirectoryHelper.GetPathRelativeToProjectDirectory("Benchmark.png"),
    options: new ReportHtmlOptions
    {
        Title = "Requested Optimization - ToPureAbsoluteUrl",
        SpectrumColumns = ["Mean", "Allocated"],
        SortByColumns = ["Mean", "Allocated"],
        HighlightGroups = false,
    });

Console.ReadLine();

#if RELEASE
[SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput)]
#endif
[HideColumns("uri")]
[MemoryDiagnoser(false)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[GroupBenchmarksBy(BenchmarkDotNet.Configs.BenchmarkLogicalGroupRule.ByParams)]
public class Benchmark
{
    public static IEnumerable<object[]> Urls
    {
        get
        {
            yield return ["Absolute", new Uri("https://sub.domain.com/segment1/segment2/resource-common-core/-/endpoint_name/216?q1=01&q2=02", UriKind.RelativeOrAbsolute)];
            //yield return ["Absolute", new Uri("https://sub.domain.com/segment1/segment2/resource-common-core/-/endpoint_name/216/", UriKind.RelativeOrAbsolute)];
            //yield return ["Relative", new Uri("/ResourceName/EndpointName?q1=1129&q2=001/", UriKind.RelativeOrAbsolute)];
            //yield return ["Relative", new Uri("/ResourceName/EndpointName/", UriKind.RelativeOrAbsolute)];
        }
    }

    [Benchmark, ArgumentsSource(nameof(Urls))]
    public string ToPureAbsoluteUrl(string name, Uri uri)
    {
        return uri.ToPureAbsoluteUrl();
    }

    [Benchmark, ArgumentsSource(nameof(Urls))]
    public string ToPureAbsoluteUrl_Optimized1(string name, Uri uri)
    {
        return uri.ToPureAbsoluteUrl_Optimized1();
    }

    [Benchmark, ArgumentsSource(nameof(Urls))]
    public string ToPureAbsoluteUrl_Optimized2(string name, Uri uri)
    {
        return uri.ToPureAbsoluteUrl_Optimized2();
    }

    [Benchmark, ArgumentsSource(nameof(Urls))]
    public string ToPureAbsoluteUrl_Optimized3(string name, Uri uri)
    {
        return uri.ToPureAbsoluteUrl_Optimized3();
    }
}

public static class UriExtensions
{
    public static string ToPureAbsoluteUrl(this Uri uri)
    {
        if (!uri.IsAbsoluteUri)
        {
            var url = uri.ToString();
            var indexOf = url.IndexOf('?', StringComparison.OrdinalIgnoreCase);
            ReadOnlySpan<char> split;

            if (indexOf > 0)
                split = url.AsSpan(0, indexOf);
            else
                split = url.AsSpan();

            return split.Trim('/').ToString();
        }

        var scapeUriBuilder = new StringBuilder();

        foreach (var segment in uri.Segments)
        {
            var param = segment.AsSpan().Trim('/');
            var numberTryParse = long.TryParse(param, CultureInfo.InvariantCulture, out _);
            var guidTryParse = Guid.TryParse(param, CultureInfo.InvariantCulture, out _);

            if (!numberTryParse && !guidTryParse)
                scapeUriBuilder.Append(segment);
        }

        return scapeUriBuilder.ToString().Trim('/');
    }

    public static string ToPureAbsoluteUrl_Optimized1(this Uri uri)
    {
        if (!uri.IsAbsoluteUri)
        {
            var url = uri.ToString();
            var indexOf = url.IndexOf('?', StringComparison.OrdinalIgnoreCase);
            ReadOnlySpan<char> split;

            if (indexOf > 0)
                split = url.AsSpan(0, indexOf);
            else
                split = url.AsSpan();

            return split.Trim('/').ToString();
        }

        var urlSpan = uri.AbsolutePath.AsSpan();

        Span<Range> ranges = stackalloc Range[urlSpan.Length];
        var writtenRanges = urlSpan.Split(ranges, '/', StringSplitOptions.RemoveEmptyEntries);
        ranges = ranges[..writtenRanges];

        Span<char> span = stackalloc char[urlSpan.Length];
        var writenCount = 0;

        foreach (var range in ranges)
        {
            var segment = urlSpan[range];

            var isLong = long.TryParse(segment, CultureInfo.InvariantCulture, out _);
            if (isLong)
                continue;

            var isGuid = Guid.TryParse(segment, CultureInfo.InvariantCulture, out _);
            if (isGuid)
                continue;

            segment.CopyTo(span[writenCount..]);
            writenCount += segment.Length;
            span[writenCount++] = '/';
        }

        var result = span[..writenCount].TrimEnd('/');
        return new string(result);
    }

    public static string ToPureAbsoluteUrl_Optimized2(this Uri uri)
    {
        if (!uri.IsAbsoluteUri)
        {
            var url = uri.ToString();
            var indexOf = url.IndexOf('?', StringComparison.OrdinalIgnoreCase);
            ReadOnlySpan<char> split;

            if (indexOf > 0)
                split = url.AsSpan(0, indexOf);
            else
                split = url.AsSpan();

            return split.Trim('/').ToString();
        }

        var urlSpan = uri.AbsolutePath.AsSpan();

        const int maxSegmentCount = 20;
        Span<Range> ranges = stackalloc Range[maxSegmentCount];
        var writtenRanges = urlSpan.Split(ranges, '/', StringSplitOptions.RemoveEmptyEntries);
        ranges = ranges[..writtenRanges];

        Span<char> span = stackalloc char[urlSpan.Length];
        var writenCount = 0;

        foreach (var range in ranges)
        {
            var segment = urlSpan[range];

            var isLong = long.TryParse(segment, CultureInfo.InvariantCulture, out _);
            if (isLong)
                continue;

            var isGuid = ValidateForGuid(segment) && Guid.TryParse(segment, CultureInfo.InvariantCulture, out _);
            if (isGuid)
                continue;

            segment.CopyTo(span[writenCount..]);
            writenCount += segment.Length;
            span[writenCount++] = '/';
        }

        var result = span[..writenCount].TrimEnd('/');
        return new string(result);
    }

    public static string ToPureAbsoluteUrl_Optimized3(this Uri uri)
    {
        //==== Use this code to handle urls such as "/ResourceName/EndpointName/216?q1=01&q2=02" ====
        //var url = uri.IsAbsoluteUri ? uri.AbsolutePath : uri.OriginalString;
        //var urlSpan = url.AsSpan();
        //var indexOf = urlSpan.IndexOf('?');
        //if (indexOf > 0)
        //    urlSpan = urlSpan[..indexOf];
        //urlSpan = urlSpan.Trim('/');

        if (!uri.IsAbsoluteUri)
        {
            var url = uri.ToString();
            var indexOf = url.IndexOf('?', StringComparison.OrdinalIgnoreCase);
            ReadOnlySpan<char> split;

            if (indexOf > 0)
                split = url.AsSpan(0, indexOf);
            else
                split = url.AsSpan();

            return split.Trim('/').ToString();
        }

        var urlSpan = uri.AbsolutePath.AsSpan().Trim('/');

        Span<char> span = stackalloc char[urlSpan.Length + 1];
        var writenLength = 0;

        var indexOfSlash = 0;
        do
        {
            indexOfSlash = urlSpan.IndexOf('/');
            var segment = indexOfSlash == -1 ? urlSpan : urlSpan[..indexOfSlash];
            urlSpan = urlSpan[(indexOfSlash + 1)..];

            var isLong = long.TryParse(segment, CultureInfo.InvariantCulture, out _);
            if (isLong)
                continue;

            var isGuid = ValidateForGuid(segment) && Guid.TryParse(segment, CultureInfo.InvariantCulture, out _);
            if (isGuid)
                continue;

            segment.CopyTo(span[writenLength..]);
            writenLength += segment.Length;
            span[writenLength++] = '/';
        } while (indexOfSlash != -1);

        var result = span[..(writenLength - 1)];
        return new string(result);
    }

    private static bool ValidateForGuid(ReadOnlySpan<char> chars)
    {
        foreach (var ch in chars)
        {
            var ascii = (int)ch;
            var isValidGuidCharacter = ascii
                is (>= 48 and <= 57) //number
                or (>= 97 and <= 102) //a-f
                or (>= 65 and <= 70) //A-F
                or 45; //-
            if (isValidGuidCharacter is false)
                return false;
        }
        return true;
    }
}