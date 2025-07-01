using System;

namespace App.Common.Adapters.Containers
{
    public interface IServiceCollectionAdapter
    {
        object GetService(Type type);
        IServiceCollectionAdapter AddTransient(Type serviceType);
        IServiceCollectionAdapter AddTransient(Type serviceType, Type implementationType);
        IServiceCollectionAdapter AddTransient<TService>(Func<TService> implementationFactory) where TService : class;
        IServiceCollectionAdapter AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IServiceCollectionAdapter AddScoped(Type serviceType);
        IServiceCollectionAdapter AddScoped(Type serviceType, Type implementationType);
        IServiceCollectionAdapter AddScoped<TService>(Func<TService> implementationFactory) where TService : class;
        IServiceCollectionAdapter AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IServiceCollectionAdapter AddSingleton(Type serviceType);
        IServiceCollectionAdapter AddSingleton(Type serviceType, Type implementationType);
        IServiceCollectionAdapter AddSingleton<TService>(Func<TService> implementationFactory) where TService : class;
        IServiceCollectionAdapter AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
    }
}
