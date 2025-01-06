using System;
using System.Linq;
using System.Reflection;
using SimpleDependencyResolver.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleDependencyResolver
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to register services using attributes.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all services marked with lifetimeS attributes from all loaded assemblies.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddSimpleDependencyResolver();
        /// </code>
        /// </example>
        public static IServiceCollection AddSimpleDependencyResolver(this IServiceCollection services)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (var index = 0; index < assemblies.Length; index++)
            {
                RegisterServicesFromAssembly(services, assemblies[index]);
            }
            
            return services;
        }

        /// <summary>
        /// Registers all services marked with lifetime attributes from the specified assembly.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="assembly">The assembly to scan for services.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddSimpleDependencyResolver(typeof(Startup).Assembly);
        /// </code>
        /// </example>
        public static IServiceCollection AddSimpleDependencyResolver(
            this IServiceCollection services,
            Assembly assembly)
        {
            RegisterServicesFromAssembly(services, assembly);

            return services;
        }

        /// <summary>
        /// Internal method to register services from a specific assembly.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="assembly">The assembly to scan for services.</param>
        private static void RegisterServicesFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } &&
                            t.GetCustomAttributes(true).Any(a => a is BaseLifetimeAttribute))
                .ToArray();

            for (var index = 0; index < types.Count(); index++)
            {
                ServiceRegistrator.Register(services, types[index]);
            }
        }
    }
}