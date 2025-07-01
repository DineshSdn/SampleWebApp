namespace App.Common.Validators.Abstractions
{
    public interface IDomainValidationRule<TModel>
        where TModel : class
    {
        string ErrorMessage { get; }
        bool IsValid(TModel instance);
    }
}
