using IMP = Aurora.Mediator.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Aurora.Mediator.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAuroraMediator(this IServiceCollection services, params Assembly[] assembliesWithHandlers)
    {
        services.AddScoped<IMediator, IMP.Mediator>();
        foreach (var assemblyWithHandlers in assembliesWithHandlers)
        {

            // Registrar Handlers de Requests
            var requestHandlers = assemblyWithHandlers.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
            foreach (var handler in requestHandlers)
            {
                var serviceType = handler.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
                services.AddScoped(serviceType, handler);
            }

            // Registrar Handlers de Notifications
            var notificationHandlers = assemblyWithHandlers.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)));
            foreach (var handler in notificationHandlers)
            {
                var serviceType = handler.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>));
                services.AddScoped(serviceType, handler);
            }
        }

        return services;
    }

    public static IServiceCollection AddPipelineBehavior<TBehavior>(this IServiceCollection services)
        where TBehavior : class
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TBehavior));
        return services;
    }
}
