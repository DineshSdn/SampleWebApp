using System;
using App.Common.Adapters.Containers;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Adapters.Containers
{
    public class ServiceCollectionAdapter : IServiceCollectionAdapter
    {
        private readonly IServiceCollection _services;

        public ServiceCollectionAdapter(IServiceCollection services)
        {
            _services = services;
        }

        public object GetService(Type type) => _services
            .BuildServiceProvider()
            .GetService(type);

        public IServiceCollectionAdapter AddTransient(Type serviceType)
        {
            _services.AddTransient(serviceType);
            return this;
        }

        public IServiceCollectionAdapter AddTransient(Type serviceType, Type implementationType)
        {
            _services.AddTransient(serviceType, implementationType);
            return this;
        }

        public IServiceCollectionAdapter AddTransient<TService>(Func<TService> implementationFactory) where TService : class
        {
            _services.AddTransient(sp => implementationFactory());
            return this;
        }

        public IServiceCollectionAdapter AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddTransient<TService, TImplementation>();
            return this;
        }

        public IServiceCollectionAdapter AddScoped(Type serviceType)
        {
            _services.AddScoped(serviceType);
            return this;
        }

        public IServiceCollectionAdapter AddScoped(Type serviceType, Type implementationType)
        {
            _services.AddScoped(serviceType, implementationType);
            return this;
        }

        public IServiceCollectionAdapter AddScoped<TService>(Func<TService> implementationFactory) where TService : class
        {
            _services.AddScoped(sp => implementationFactory());
            return this;
        }

        public IServiceCollectionAdapter AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddScoped<TService, TImplementation>();
            return this;
        }

        public IServiceCollectionAdapter AddSingleton(Type serviceType)
        {
            _services.AddSingleton(serviceType);
            return this;
        }

        public IServiceCollectionAdapter AddSingleton(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
            return this;
        }

        public IServiceCollectionAdapter AddSingleton<TService>(Func<TService> implementationFactory) where TService : class
        {
            _services.AddSingleton(sp => implementationFactory());
            return this;
        }

        public IServiceCollectionAdapter AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _services.AddSingleton<TService, TImplementation>();
            return this;
        }
    }
}
