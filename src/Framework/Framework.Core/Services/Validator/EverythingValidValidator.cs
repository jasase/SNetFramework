using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.Validator
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
