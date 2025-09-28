using Aurora.Mediator;

namespace Aurora.Mediator.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            Console.WriteLine($"➡️ Handling {typeof(TRequest).Name} with data: {@request}");
    
            var response = await next();

            Console.WriteLine($"✅ Handled {typeof(TRequest).Name}. Response: {System.Text.Json.JsonSerializer.Serialize(response)}");

            return response;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"❌ An exception occurred during handling processing {typeof(TRequest).Name}: {ex.Message}");
            throw;
        }
    }
}