using System;
using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class MapsterExtensions
    {
        public static IServiceCollection AddMapster
        (
            this IServiceCollection services,
            Action<TypeAdapterConfig> options = null
        )
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.Scan(Assembly.GetExecutingAssembly());

            options?.Invoke(config);

            services.AddSingleton(config);
            services.AddSingleton<IMapper, ServiceMapper>();

            return services;
        }
    }
}
