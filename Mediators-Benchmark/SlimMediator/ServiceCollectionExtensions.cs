using Microsoft.Extensions.DependencyInjection.Extensions;
using SlimMediator;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSlimMediator(this IServiceCollection services, Action<SlimMediatorOptions> configure = null)
    {
        services.TryAddSingleton<ISender, Sender>();

        var options = new SlimMediatorOptions();
        configure?.Invoke(options);

        if (options.AssembliesToRegister.Count > 0)
            services.AddHandlersFromAssemblies(options.Lifetime, options.AssembliesToRegister.ToArray());

        return services;
    }

    private static IServiceCollection AddHandlersFromAssemblies(this IServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
    {
        var handlerTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Select(type =>
            {
                var genericArguments = type.GetInterfaces()
                    .Where(@interface => @interface.IsGenericType)
                    .Select(@interface =>
                    {
                        var genericType = @interface.GetGenericTypeDefinition();
                        return genericType == typeof(IRequestHandler<,>) || genericType == typeof(IRequestHandler<>) ? @interface : null;
                    })
                    .Where(@interface => @interface is not null)
                    .SelectMany(@interface => @interface.GetGenericArguments())
                    .ToArray();

                return (HandlerType: type, GenericArguments: genericArguments);
            })
            .Where(x => x.GenericArguments.Length != 0)
            .ToArray();

        foreach (var (HandlerType, GenericArguments) in handlerTypes)
        {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning disable CS0436 // Type conflicts with imported type
            var methodInfo = typeof(ServiceCollectionExtensions)
                .GetMethod(nameof(AddHandler), GenericArguments.Length + 1, BindingFlags.NonPublic | BindingFlags.Static, null, [typeof(IServiceCollection), typeof(ServiceLifetime)], null);
#pragma warning restore CS0436 // Type conflicts with imported type
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

#pragma warning disable S3878 // Arrays should not be created for params parameters
            var method = methodInfo.MakeGenericMethod([.. GenericArguments, HandlerType]);
#pragma warning restore S3878 // Arrays should not be created for params parameters

            method?.Invoke(services, [services, lifetime]);
        }

        return services;
    }

    internal static IServiceCollection AddHandler<TRequest, THandler>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TRequest : class, IRequest
        where THandler : class, IRequestHandler<TRequest>
    {
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(IRequestHandler<TRequest>), typeof(THandler), lifetime);
        services.Add(serviceDescriptor);
        return services;
    }

    internal static IServiceCollection AddHandler<TRequest, TResponse, THandler>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TRequest : class, IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        var serviceDescriptor = ServiceDescriptor.Describe(typeof(IRequestHandler<TRequest, TResponse>), typeof(THandler), lifetime);
        services.Add(serviceDescriptor);
        return services;
    }
}