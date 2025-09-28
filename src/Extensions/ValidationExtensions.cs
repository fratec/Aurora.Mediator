using System.Reflection;
using Aurora.Mediator.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.Mediator.Extensions;

public static class ValidationExtensions
{
    /// <summary>
    /// Ativa o ValidationBehavior no pipeline do Aurora.Mediator.
    /// </summary>
    public static IServiceCollection UseDefaultValidation(this IServiceCollection services, params Assembly[] assembliesWithValidators)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

         if (assembliesWithValidators != null && assembliesWithValidators.Length > 0)
        {
            foreach (var assembly in assembliesWithValidators)
            {
                var validatorTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                    .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == typeof(IValidator<>));

                foreach (var v in validatorTypes)
                {
                    services.AddScoped(v.Interface, v.Type);
                }
            }
        }
        return services;
    }
}