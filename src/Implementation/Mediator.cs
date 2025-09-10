using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Mediator.Implementation;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBus? _eventBus;

    public Mediator(IServiceProvider serviceProvider, IEventBus? eventBus = null)
    {
        _serviceProvider = serviceProvider;
        _eventBus = eventBus;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler não encontrado para {request.GetType().Name}");

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var behaviors = _serviceProvider.GetServices(behaviorType).Cast<object>().ToList();

        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
            (Task<TResponse>)handlerType.GetMethod("Handle")!
                .Invoke(handler, new object[] { request, cancellationToken })!;

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var current = handlerDelegate;
            handlerDelegate = () =>
                (Task<TResponse>)behavior.GetType()
                    .GetMethod("Handle")!
                    .Invoke(behavior, new object[] { request, cancellationToken, current })!;
        }

        return await handlerDelegate();
    }

    public async Task Publish<TNotification>(TNotification notification, bool sendToBus = false, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        // 1. Chama handlers internos
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = _serviceProvider.GetServices(handlerType).Cast<object>();
        foreach (var handler in handlers)
        {
            await (Task)handlerType.GetMethod("Handle")!
                .Invoke(handler, new object[] { notification, cancellationToken })!;
        }

        // 2. EventBus externo (Azure, RabbitMQ, Kafka)
        if (_eventBus != null && sendToBus)
        {
            await _eventBus.PublishAsync(notification, cancellationToken);
        }
    }
}