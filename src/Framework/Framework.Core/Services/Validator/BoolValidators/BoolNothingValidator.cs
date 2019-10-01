using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.Validator.BoolValidators
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
