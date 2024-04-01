namespace SlimMediator;

public interface ISender
{
#pragma warning disable AMNF0001 // Awaitable (Asynchronous) methods should be suffixed with 'Async'
    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : class, IRequest;
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
#pragma warning restore AMNF0001 // Awaitable (Asynchronous) methods should be suffixed with 'Async'
}