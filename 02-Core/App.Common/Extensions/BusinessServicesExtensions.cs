using System.Reflection;
using App.Common.Adapters.Containers;
using App.Common.Attributes;
using App.Common.Helpers;

namespace App.Common.Extensions
{
    public static class BusinessServicesExtensions
    {
        public static IServiceCollectionAdapter RegisterServicesForAssemblies(this IServiceCollectionAdapter services, params Assembly[] assemblies)
        {
            services.RegisterTransientServices(assemblies);
            services.RegisterScopedServices(assemblies);
            services.RegisterSingletonServices(assemblies);

            return services;
        }

        private static IServiceCollectionAdapter RegisterTransientServices(this IServiceCollectionAdapter services, params Assembly[] assemblies)
        {
            var mappedTypes = MappedTypesFactory.GetMappedTypesFromAssemblies<TransientServiceAttribute>(assemblies);

            foreach (var mappedType in mappedTypes)
                services.AddTransient(mappedType.Key, mappedType.Value);

            return services;
        }

        private static IServiceCollectionAdapter RegisterScopedServices(this IServiceCollectionAdapter services, params Assembly[] assemblies)
        {
            var mappedTypes = MappedTypesFactory.GetMappedTypesFromAssemblies<ScopedServiceAttribute>(assemblies);

            foreach (var mappedType in mappedTypes)
                services.AddScoped(mappedType.Key, mappedType.Value);

            return services;
        }

        private static IServiceCollectionAdapter RegisterSingletonServices(this IServiceCollectionAdapter services, params Assembly[] assemblies)
        {
            var mappedTypes = MappedTypesFactory.GetMappedTypesFromAssemblies<SingletonServiceAttribute>(assemblies);

            foreach (var mappedType in mappedTypes)
                services.AddSingleton(mappedType.Key, mappedType.Value);

            return services;
        }
    }
}
