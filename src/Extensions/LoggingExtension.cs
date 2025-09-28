using Aurora.Mediator.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Mediator.Extensions;

public static class LoggingExtension
{
    /// <summary>
    /// Ativa o LoggingBehavior no pipeline do Aurora.Mediator.
    /// </summary>
    public static IServiceCollection UseDefaultLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}
