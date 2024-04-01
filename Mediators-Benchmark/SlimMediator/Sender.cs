using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace SlimMediator;

public class Sender(IServiceProvider serviceProvider) : ISender
{
    private static readonly ConcurrentDictionary<Type, Type> _requestHandlers = new();

    public Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : class, IRequest
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        return handler.Handle(request, cancellationToken);
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = _requestHandlers.GetOrAdd(request.GetType(), requestType => typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse)));
        var handler = serviceProvider.GetService(handlerType);
        var getResponse = (IGetResponse<TResponse>)handler;
        return getResponse.GetResponse(request, cancellationToken);
    }
}