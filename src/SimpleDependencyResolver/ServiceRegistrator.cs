using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SimpleDependencyResolver.Attributes;

namespace SimpleDependencyResolver;

/// <summary>
/// Provides functionality to register services in the dependency injection container based on attributes.
/// </summary>
/// <remarks>
/// This static class handles the registration of services with different lifetimes and supports both
/// standard and keyed service registration in .NET dependency injection.
/// </remarks>
public static class ServiceRegistrator
{
    /// <summary>
    /// Registers a service type in the dependency injection container based on its lifetime attributes.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="implementationType">The implementation type to register.</param>
    /// <remarks>
    /// This method supports registration of services with or without interfaces, and with different lifetimes (Transient, Scoped, Singleton).
    /// For .NET 8 and above, it also supports keyed services registration.
    /// </remarks>
    /// <example>
    /// <code>
    /// [Scoped]
    /// public class MyService : IMyService 
    /// {
    /// }
    /// 
    /// // Register the service
    /// ServiceRegistrator.Register(services, typeof(MyService));
    /// </code>
    /// </example>
    /// <exception cref="InvalidOperationException">Thrown when the type does not have a lifetime attribute.</exception>
    public static void Register(IServiceCollection services, Type implementationType)
    {
        var attribute = implementationType
            .GetCustomAttributes(true)
            .FirstOrDefault(a => a is BaseLifetimeAttribute) as BaseLifetimeAttribute;

        var interfaceType = implementationType.GetInterfaces().FirstOrDefault();
#if NET8_0_OR_GREATER
        var withServiceKey = attribute!.ServiceKey != null;
#else
        var withServiceKey = false;
#endif
        var implementsInterface = interfaceType != null;

        switch (withServiceKey)
        {
            case false:
                RegisterService(services, attribute!, implementsInterface, interfaceType, implementationType);
                break;
#if NET8_0_OR_GREATER
            case true:
                RegisterKeyedService(services, attribute, implementsInterface, interfaceType, implementationType);
                break;
#endif
        }
    }

    private static void RegisterService(
        IServiceCollection services,
        BaseLifetimeAttribute attribute,
        bool implementsInterface,
        Type? interfaceType,
        Type implementationType)
    {
        switch (attribute, implementsInterface)
        {
            case (TransientAttribute, false):
                services.AddTransient(implementationType);
                break;
            case (TransientAttribute, true):
                services.AddTransient(interfaceType!, implementationType);
                break;

            case (ScopedAttribute, false):
                services.AddScoped(implementationType);
                break;
            case (ScopedAttribute, true):
                services.AddScoped(interfaceType!, implementationType);
                break;

            case (SingletonAttribute, false):
                services.AddSingleton(implementationType);
                break;
            case (SingletonAttribute, true):
                services.AddSingleton(interfaceType!, implementationType);
                break;
        }
    }

#if NET8_0_OR_GREATER
    private static void RegisterKeyedService(
        IServiceCollection services,
        BaseLifetimeAttribute attribute,
        bool implementsInterface,
        Type? interfaceType,
        Type implementationType)
    {
        switch (attribute, implementsInterface)
        {
            case (TransientAttribute, false):
                services.AddKeyedTransient(implementationType, attribute!.ServiceKey);
                break;
            case (TransientAttribute, true):
                services.AddKeyedTransient(interfaceType!, attribute!.ServiceKey, implementationType);
                break;

            case (ScopedAttribute, false):
                services.AddKeyedScoped(implementationType, attribute!.ServiceKey);
                break;
            case (ScopedAttribute, true):
                services.AddKeyedScoped(interfaceType!, attribute!.ServiceKey, implementationType);
                break;

            case (SingletonAttribute, false):
                services.AddKeyedSingleton(implementationType, attribute!.ServiceKey);
                break;
            case (SingletonAttribute, true):
                services.AddKeyedSingleton(interfaceType!, attribute!.ServiceKey, implementationType);
                break;
        }
    }
#endif
}