namespace SlimMediator;

public interface ISender
{
    public Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : class, IRequest;
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}