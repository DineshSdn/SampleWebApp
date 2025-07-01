using System.Reflection;
using App.Common.Abstractions.DbDrivers;
using App.Common.Extensions;
using App.Core;
using Infrastructure.Adapters.Containers;
using Infrastructure.Extensions;
using Infrastructure.Implementations.DbDrivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterBusinessAndInfrastructureServicess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapster();

            services.AddDbContext<AppDbContext>
            (
                (provider, options) =>
                {
                    var masterConnectionString = configuration.GetConnectionString("DefaultConnection");

                    options.UseSqlServer
                    (
                      masterConnectionString,
                      b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                    );
                }
            );

            services.AddTransient<IAppDbContext, AppDbContext>();

            var serviceCollectionAdapter = new ServiceCollectionAdapter(services);
            serviceCollectionAdapter.RegisterServicesForAssemblies(Assembly.GetExecutingAssembly());

            // Register Business Services
            serviceCollectionAdapter.AddApplicationDependencies();

            return services;
        }
    }
}
