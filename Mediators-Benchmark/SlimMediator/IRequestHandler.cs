namespace SlimMediator;

#pragma warning disable AMNF0001 // Awaitable (Asynchronous) methods should be suffixed with 'Async'
public interface IRequestHandler<in TRequest>
    where TRequest : class, IRequest
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IGetResponse<TResponse>
{
    Task<TResponse> GetResponse<T>(T request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest, TResponse> : IGetResponse<TResponse>
    where TRequest : class, IRequest<TResponse>
{
    Task<TResponse> IGetResponse<TResponse>.GetResponse<T>(T request, CancellationToken cancellationToken) => Handle(request as TRequest, cancellationToken);
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
#pragma warning restore AMNF0001 // Awaitable (Asynchronous) methods should be suffixed with 'Async'
