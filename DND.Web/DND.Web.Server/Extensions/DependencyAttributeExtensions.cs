using System;
using System.Linq;
using System.Reflection;
using DND.Middleware.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DND.Web.Server.Extensions
{
    public static class DependencyAttributeExtensions
    {
        public static void RegisterServicesWithAttributes(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var targetServices = assembly.GetTypes().Where(type =>
                type.GetCustomAttribute<ScopedDependencyAttribute>() != null ||
                type.GetCustomAttribute<TransientDependencyAttribute>() != null ||
                type.GetCustomAttribute<SingletonDependencyAttribute>() != null
            );

            foreach (var serviceType in targetServices)
            {
                // Get all interface that the class implemented
                var implementedInterfaces = serviceType.GetInterfaces();

                // Get the lifetime of the class
                var lifetime = GetLifetimeFromAttribute(serviceType);

                // Class implemented interface
                if (implementedInterfaces.Length != 0)
                {
                    foreach (var @interface in implementedInterfaces)
                    {
                        RegisterService(serviceCollection, @interface, serviceType, lifetime);
                    }
                }
                else
                {
                    // Class doesn't implemented interface
                    RegisterService(serviceCollection, null, serviceType, lifetime);
                }
            }
        }

        private static void RegisterService(IServiceCollection serviceCollection, Type @interface, Type serviceType, ServiceLifetime lifetime)
        {
            _ = lifetime switch
            {
                ServiceLifetime.Singleton => @interface != null ? serviceCollection.AddSingleton(@interface, serviceType) : serviceCollection.AddSingleton(serviceType),
                ServiceLifetime.Scoped => @interface != null ? serviceCollection.AddScoped(@interface, serviceType) : serviceCollection.AddScoped(serviceType),
                ServiceLifetime.Transient => @interface != null ? serviceCollection.AddTransient(@interface, serviceType) : serviceCollection.AddTransient(serviceType),
                _ => @interface != null ? serviceCollection.AddScoped(@interface, serviceType) : serviceCollection.AddScoped(serviceType),
            };
        }

        private static ServiceLifetime GetLifetimeFromAttribute(Type serviceType)
        {
            if (serviceType.GetCustomAttribute<ScopedDependencyAttribute>() != null)
            {
                return ServiceLifetime.Scoped;
            }

            if (serviceType.GetCustomAttribute<TransientDependencyAttribute>() != null)
            {
                return ServiceLifetime.Transient;
            }

            if (serviceType.GetCustomAttribute<SingletonDependencyAttribute>() != null)
            {
                return ServiceLifetime.Singleton;
            }

            return ServiceLifetime.Scoped;
        }
    }
}