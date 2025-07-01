using System.Reflection;
using App.Common.Adapters.Containers;
using App.Common.Validators.Helpers;

namespace App.Common.Extensions
{
    public static class DomainValidatorsExtensions
    {
        public static IServiceCollectionAdapter RegisterValidatorsFromAssemblies(this IServiceCollectionAdapter services, params Assembly[] assemblies)
        {
            DomainValidatorHelper.RegisterValidatorsFromAssemblies((x, y) => services.AddTransient(x, y), assemblies);

            return services;
        }
    }
}
