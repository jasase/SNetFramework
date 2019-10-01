using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;

namespace Framework.Core.Services.Validator.BoolValidators
{
    public class BoolNothingValidator<TEntity> : BaseBoolValidator<TEntity>
        where TEntity : Entity
    {
        public override ValidationResult Validate(TEntity entity, bool validate)
        {
            return ValidationResult.Valid();
        }
    }
}
