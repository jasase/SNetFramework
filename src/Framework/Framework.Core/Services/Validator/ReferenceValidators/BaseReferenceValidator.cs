using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.Validator.ReferenceValidators
{
    public class BaseReferenceValidator<TEntity> : BaseValidator<EntityReference, TEntity>
        where TEntity : Entity
    {
        private bool _allowEmpty;

        public BaseReferenceValidator(bool allowEmpty)
        {
            _allowEmpty = allowEmpty;
        }

        public override ValidationResult Validate(TEntity entity, EntityReference validate)
        {
            if (!_allowEmpty && validate == null)
            {
                return ValidationResult.Invalid("Eine leere Referenze ist nicht erlaubt");
            }
            return ValidationResult.Valid();
        }
    }
}
