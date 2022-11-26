using System.Linq.Expressions;
namespace PropertyGetterBenchmark;

public static class ExpressionFetcher
{
    private static Func<object, HttpRequestMessage> _requestGetterFunction;
    private static Func<object, HttpResponseMessage> _responseGetterFunction;

    public static HttpRequestMessage GetRequestMessage(object payload)
    {
        LazyInitializer.EnsureInitialized(ref _requestGetterFunction, () =>
        {
            var payloadType = payload.GetType();
            return GetPropertyGetterFunction<HttpRequestMessage>(payloadType, "Request");
        });

        return _requestGetterFunction(payload);
    }

    public static HttpResponseMessage GetResponseMessage(object payload)
    {
        LazyInitializer.EnsureInitialized(ref _responseGetterFunction, () =>
        {
            var payloadType = payload.GetType();
            return GetPropertyGetterFunction<HttpResponseMessage>(payloadType, "Response");
        });

        return _responseGetterFunction(payload);
    }

    // https://source.dot.net/#System.Net.Http/System/Net/Http/DiagnosticsHandler.cs,3e6ad991d2a03b5c,references
    private static Func<object, TResult> GetPropertyGetterFunction<TResult>(Type payloadType, string propertyName)
    {
        var objectParameterExpression = Expression.Parameter(typeof(object));
        var typeConversionExpression = Expression.Convert(objectParameterExpression, payloadType);
        var accessPropertyExpression = Expression.Property(typeConversionExpression, propertyName);
        return Expression.Lambda<Func<object, TResult>>(accessPropertyExpression, objectParameterExpression).Compile();
    }
}