using Framework.Abstraction.Services;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator
{
    public abstract class BaseValidator<TValidate, TEntity> : IValidator<TValidate, TEntity>
        where TEntity : Entity
    {
        public ValidationResult Validate(Entity entity, TValidate newValue)
        {
            var typedEntity = entity as TEntity;
            if (typedEntity != null)
            {
                return Validate(typedEntity, newValue);
            }
            return ValidationResult.Invalid(string.Format("Provided entity of type [{0}] has not the expected type [{1}]",
                entity.GetType().FullName,
                typeof(TEntity).FullName));
        }

        public abstract ValidationResult Validate(TEntity entity, TValidate newValue);
    }
}
