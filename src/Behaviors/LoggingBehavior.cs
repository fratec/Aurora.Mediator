using Aurora.Mediator;

namespace Aurora.Mediator.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Console.WriteLine("➡️ Handling {RequestName} with data: {@Request}", typeof(TRequest).Name, request);

        var response = await next();

        Console.WriteLine("✅ Handled {RequestName}. Response: {@Response}", typeof(TRequest).Name, response);

        return response;
    }
}