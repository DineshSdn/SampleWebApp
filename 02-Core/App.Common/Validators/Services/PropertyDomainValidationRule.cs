using System;
using App.Common.Validators.Abstractions;

namespace App.Common.Validators.Services
{
    public sealed class PropertyDomainValidationRule<TModel, TProperty> : IDomainValidationRule<TModel>
        where TModel : class
    {
        private readonly Func<TModel, TProperty> _propertySelector;
        private readonly Func<TProperty, bool> _predicate;

        public string ErrorMessage { get; } = string.Empty;

        public PropertyDomainValidationRule
        (
            Func<TModel, TProperty> propertySelector,
            Func<TProperty, bool> predicate,
            string errorMessage
        )
        {
            _propertySelector = propertySelector;
            _predicate = predicate;
            ErrorMessage = errorMessage;
        }

        public bool IsValid(TModel model)
        {
            var property = _propertySelector(model);
            return _predicate.Invoke(property);
        }
    }
}
