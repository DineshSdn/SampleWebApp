using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Validators.Abstractions;
using App.Common.Validators.Models;

namespace App.Common.Validators.Services
{
    public abstract class AbstractDomainValidator<TModel> : IDomainValidator<TModel>
        where TModel : class
    {
        private readonly List<IDomainValidationRule<TModel>> _rules = new List<IDomainValidationRule<TModel>>();
        private readonly List<string> _validationErrors = new List<string>();

        public ValidationResultDto Validate(TModel model)
        {
            if (model == null)
                _validationErrors.Add($"Object of type {typeof(TModel).Name} is null.");

            if (_validationErrors.Count > 0)
                return new ValidationResultDto
                {
                    Errors = _validationErrors
                };

            foreach (var rule in _rules.Where(r => !r.IsValid(model)))
                _validationErrors.Add(rule.ErrorMessage);

            return new ValidationResultDto
            {
                Errors = _validationErrors
            };
        }

        public DomainRuleBuilder<TModel, TProperty> RuleFor<TProperty>(Func<TModel, TProperty> propertySelector)
        {
            return new DomainRuleBuilder<TModel, TProperty>(propertySelector, _rules);
        }
    }
}
