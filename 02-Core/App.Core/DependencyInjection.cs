using System.Reflection;
using App.Common.Adapters.Containers;
using App.Common.Extensions;

namespace App.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollectionAdapter AddApplicationDependencies(this IServiceCollectionAdapter services)
        {

            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterServicesForAssemblies(assembly);
            services.RegisterValidatorsFromAssemblies(assembly);

            return services;
        }
    }
}
