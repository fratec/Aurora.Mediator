using Aurora.Mediator.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Mediator.Extensions;

public static class ValidationExtensions
{
    /// <summary>
    /// Ativa o ValidationBehavior no pipeline do Aurora.Mediator.
    /// </summary>
    public static IServiceCollection UseDefaultValidation(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}