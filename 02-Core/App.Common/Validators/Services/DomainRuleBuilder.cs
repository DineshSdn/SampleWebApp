using System;
using System.Collections.Generic;
using App.Common.Validators.Abstractions;

namespace App.Common.Validators.Services
{
    public sealed class DomainRuleBuilder<TModel, TProperty>
        where TModel : class
    {
        private readonly Func<TModel, TProperty> _propertyFunc;
        private readonly List<IDomainValidationRule<TModel>> _rules;

        public DomainRuleBuilder(Func<TModel, TProperty> propertyFunc, List<IDomainValidationRule<TModel>> rules)
        {
            _propertyFunc = propertyFunc;
            _rules = rules;
        }

        public DomainRuleBuilder<TModel, TProperty> Must(Func<TProperty, bool> predicate, string errorMessage)
        {
            _rules.Add(new PropertyDomainValidationRule<TModel, TProperty>(_propertyFunc, predicate, errorMessage));
            return this;
        }
    }
}
