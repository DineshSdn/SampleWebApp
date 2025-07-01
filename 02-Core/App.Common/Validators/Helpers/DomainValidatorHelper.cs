using System;
using System.Linq;
using System.Reflection;
using App.Common.Validators.Abstractions;
using App.Common.Validators.Services;

namespace App.Common.Validators.Helpers
{
    public static class DomainValidatorHelper
    {
        public static void RegisterValidatorsFromAssemblies(Action<Type, Type> registrationAction, params Assembly[] assemblies)
        {
            var genericType = typeof(AbstractDomainValidator<>);

            var types = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(type => !type.IsAbstract && !type.IsInterface && !type.IsGenericTypeDefinition)
                .Where(type => type.BaseType != null
                        && type.BaseType.IsGenericType
                        && type.BaseType.GetGenericTypeDefinition() == genericType)
                .Select(x => GetTypeMap(x))
                .ToList();

            foreach (var typeMap in types)
                registrationAction(typeMap.requestType, typeMap.validatorType);
        }

        private static (Type requestType, Type validatorType) GetTypeMap(TypeInfo info)
        {
            var requestType = info.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainValidator<>));

            return (requestType, info.UnderlyingSystemType);
        }
    }
}
