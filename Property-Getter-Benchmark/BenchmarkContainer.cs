using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using System.Reflection;

//[DryJob(RuntimeMoniker.NetCoreApp31)]
//[LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
//[SimpleJob]
//[SimpleJob(RunStrategy.Throughput)]
//[SimpleJob(RuntimeMoniker.NetCoreApp31)]
//[InProcess(true)]
//[NativeMemoryProfiler]
//[ThreadingDiagnoser]
//[ConcurrencyVisualizerProfiler]
//[TailCallDiagnoser]
//[InliningDiagnoser(true, true)]
//[EtwProfiler]
//[DisassemblyDiagnoser] //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
//[CategoriesColumn, GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
//[RPlotExporter]
[ShortRunJob]
//[DryJob]
[MemoryDiagnoser]
[KeepBenchmarkFiles(false)]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
[RankColumn, Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class BenchmarkContainer
{
    private readonly object _startData;
    private readonly object _stopData;

    private readonly Type _activityStartDataType;
    private readonly Type _activityStopDataType;

    private readonly Func<object, HttpRequestMessage> _requestAccessor;
    private readonly Func<object, HttpResponseMessage> _responseAccessor;

    private readonly PropertyFetcher<HttpRequestMessage> _requestPropertyFetcher;
    private readonly PropertyFetcher<HttpResponseMessage> _responsePropertyFetcher;

    private readonly PropertyFetcher2<HttpRequestMessage> _requestPropertyFetcher2;
    private readonly PropertyFetcher2<HttpResponseMessage> _responsePropertyFetcher2;

    //[GlobalSetup]
    //public void Setup()
    public BenchmarkContainer()
    {
        _startData = HttpDiagnostic.GetActivityStartData();
        _stopData = HttpDiagnostic.GetActivityStopData();

        _activityStartDataType = Type.GetType("System.Net.Http.HttpDiagnostic+ActivityStartData, PropertyGetterBenchmark", throwOnError: true);
        _activityStopDataType = Type.GetType("System.Net.Http.HttpDiagnostic+ActivityStopData, PropertyGetterBenchmark", throwOnError: true);


        _requestAccessor = FieldGetter<HttpRequestMessage>.Create(_activityStartDataType, "Request", BindingFlags.Public | BindingFlags.Instance);
        _responseAccessor = FieldGetter<HttpResponseMessage>.Create(_activityStopDataType, "Response", BindingFlags.Public | BindingFlags.Instance);

        _requestPropertyFetcher = new PropertyFetcher<HttpRequestMessage>("Request");
        _responsePropertyFetcher = new PropertyFetcher<HttpResponseMessage>("Response");

        _requestPropertyFetcher2 = new PropertyFetcher2<HttpRequestMessage>("Request");
        _responsePropertyFetcher2 = new PropertyFetcher2<HttpResponseMessage>("Response");
    }

    [Benchmark]
    public (HttpRequestMessage, HttpResponseMessage) FieldGetter()
    {
        var httpRequest = _requestAccessor(_startData);
        var httpResponse = _responseAccessor(_stopData);
        return (httpRequest, httpResponse);
    }

    [Benchmark]
    public (HttpRequestMessage, HttpResponseMessage) PropertyFetcher()
    {
        var httpRequest = _requestPropertyFetcher.Fetch(_startData);
        var httpResponse = _responsePropertyFetcher.Fetch(_stopData);
        return (httpRequest, httpResponse);
    }

    [Benchmark]
    public (HttpRequestMessage, HttpResponseMessage) PropertyFetcher2()
    {
        var httpRequest = _requestPropertyFetcher2.Fetch(_startData);
        var httpResponse = _responsePropertyFetcher2.Fetch(_stopData);
        return (httpRequest, httpResponse);
    }

    [Benchmark]
    public (HttpRequestMessage, HttpResponseMessage) ExpressionFetcher()
    {
        var httpRequest = PropertyGetterBenchmark.ExpressionFetcher.GetRequestMessage(_startData);
        var httpResponse = PropertyGetterBenchmark.ExpressionFetcher.GetResponseMessage(_stopData);
        return (httpRequest, httpResponse);
    }

    [Benchmark]
    public (HttpRequestMessage, HttpResponseMessage) ReflectionFetcher()
    {
        var httpRequest = PropertyGetterBenchmark.ReflectionFetcher.RequestAccessor(_startData);
        var httpResponse = PropertyGetterBenchmark.ReflectionFetcher.ResponseAccessor(_stopData);
        return (httpRequest, httpResponse);
    }
}

namespace System.Net.Http
{
    public class HttpDiagnostic
    {
        public static object GetActivityStartData()
        {
            return new ActivityStartData(new(HttpMethod.Get, "https://www.google.com/"));
        }
        public static object GetActivityStopData()
        {
            return new ActivityStopData(new(HttpStatusCode.InternalServerError));
        }

        private sealed class ActivityStartData
        {
            internal ActivityStartData(HttpRequestMessage request)
            {
                Request = request;
            }

            public HttpRequestMessage Request { get; }
        }

        private sealed class ActivityStopData
        {
            internal ActivityStopData(HttpResponseMessage response)
            {
                Response = response;
            }

            public HttpResponseMessage Response { get; }
        }
    }
}