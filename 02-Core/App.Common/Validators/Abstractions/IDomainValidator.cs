using App.Common.Validators.Models;

namespace App.Common.Validators.Abstractions
{
    public interface IDomainValidator<TModel>
         where TModel : class
    {
        ValidationResultDto Validate(TModel model);
    }
}
