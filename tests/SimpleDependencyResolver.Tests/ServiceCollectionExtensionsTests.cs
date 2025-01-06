using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleDependencyResolver.Attributes;

namespace SimpleDependencyResolver.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Theory]
        [InlineData(typeof(ITransientService), typeof(TransientService))]
        [InlineData(typeof(IScopedService), typeof(ScopedService))]
        [InlineData(typeof(ISingletonService), typeof(SingletonService))]
        public void AddSimpleDependencyResolver_ShouldRegisterServicesWithAttributes(
            Type serviceType,
            Type implementationType)
        {
            // Arrange
            var services = new ServiceCollection();
            var assembly = Assembly.GetExecutingAssembly();

            // Act
            services.AddSimpleDependencyResolver(assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service = serviceProvider.GetRequiredService(serviceType);

            service.Should().BeOfType(implementationType);
        }

        [Theory]
        [InlineData(typeof(ITransientService), typeof(TransientService))]
        [InlineData(typeof(IScopedService), typeof(ScopedService))]
        [InlineData(typeof(ISingletonService), typeof(SingletonService))]
        public void AddSimpleDependencyResolver_ShouldRegisterServicesFromAllAssemblies(
            Type serviceType,
            Type implementationType)
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSimpleDependencyResolver();
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service = serviceProvider.GetRequiredService(serviceType);

            service.Should().BeOfType(implementationType);
        }

        [Theory]
        [InlineData(typeof(ITransientService), typeof(KeyedTransientService), "TransientKey")]
        [InlineData(typeof(IScopedService), typeof(KeyedScopedService), "ScopedKey")]
        [InlineData(typeof(ISingletonService), typeof(KeyedSingletonService), "SingletonKey")]
        public void AddSimpleDependencyResolver_ShouldRegisterKeyedServices(
            Type serviceType,
            Type implementationType,
            string serviceKey)
        {
            // Arrange
            var services = new ServiceCollection();
            var assembly = Assembly.GetExecutingAssembly();

            // Act
            services.AddSimpleDependencyResolver(assembly);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var service = serviceProvider.GetRequiredKeyedService(serviceType, serviceKey);

            service.Should().BeOfType(implementationType);
        }
    }

    public interface ITransientService { }
    public interface IScopedService { }
    public interface ISingletonService { }

    [Transient]
    public class TransientService : ITransientService { }

    [Scoped]
    public class ScopedService : IScopedService { }

    [Singleton]
    public class SingletonService : ISingletonService { }
    
    [Transient(ServiceKey = "TransientKey")]
    public class KeyedTransientService : ITransientService { }

    [Scoped(ServiceKey = "ScopedKey")]
    public class KeyedScopedService : IScopedService { }

    [Singleton(ServiceKey = "SingletonKey")]
    public class KeyedSingletonService : ISingletonService { }
}