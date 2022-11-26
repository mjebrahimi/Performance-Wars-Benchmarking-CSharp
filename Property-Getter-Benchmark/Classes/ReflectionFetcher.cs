namespace PropertyGetterBenchmark;

public class ReflectionFetcher
{
    public static readonly Func<object, HttpRequestMessage> RequestAccessor = CreateGetRequestFunc();
    public static readonly Func<object, HttpResponseMessage> ResponseAccessor = CreateGetResponseFunc();

    private static Func<object, HttpRequestMessage> CreateGetRequestFunc()
    {
        var type = Type.GetType("System.Net.Http.HttpDiagnostic+ActivityStartData, PropertyGetterBenchmark", throwOnError: true);
        var prop = type.GetProperty("Request");
        return (object o) => (HttpRequestMessage)prop.GetValue(o);
    }

    private static Func<object, HttpResponseMessage> CreateGetResponseFunc()
    {
        var type = Type.GetType("System.Net.Http.HttpDiagnostic+ActivityStopData, PropertyGetterBenchmark", throwOnError: true);
        var prop = type.GetProperty("Response");
        return (object o) => (HttpResponseMessage)prop.GetValue(o);
    }
}
