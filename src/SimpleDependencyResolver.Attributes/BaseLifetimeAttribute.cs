using System;

namespace SimpleDependencyResolver.Attributes
{
    /// <summary>
    /// Define a base attribute for dependency resolvers
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public abstract class BaseLifetimeAttribute : Attribute
    {
#if NET8_0_OR_GREATER
        /// <summary>
        /// The key of the service in ServiceDescriptor.ServiceKey
        /// </summary>
        public object? ServiceKey { get; set; }
#endif
    }
}
