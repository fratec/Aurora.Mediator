namespace Aurora.Mediator;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    Task Publish<TNotification>(TNotification notification, bool sendToBus = false, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}
