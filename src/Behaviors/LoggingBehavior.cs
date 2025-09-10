using Aurora.Mediator;

namespace Aurora.Mediator.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine($"[LOG] Started {typeof(TRequest).Name}");
        var response = await next();
        Console.WriteLine($"[LOG] Ended {typeof(TRequest).Name}");
        return response;
    }
}