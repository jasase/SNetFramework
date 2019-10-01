using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;

namespace Framework.Core.Services.Validator
{
    public class EverythingValidValidator<TValidate, TEntity> : BaseValidator<TValidate, TEntity>
        where TEntity : Entity
    {
        public override ValidationResult Validate(TEntity entity, TValidate newValue)
        {
            return ValidationResult.Valid();
        }
    }
}
