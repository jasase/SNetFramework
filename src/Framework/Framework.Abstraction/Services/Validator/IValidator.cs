using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;

namespace Framework.Abstraction.Services
{
    public interface IValidator<in TValidate>
    {
        ValidationResult Validate(Entity entity, TValidate newValue);
    }

    public interface IValidator<in TValidate, in TEntity> : IValidator<TValidate>
        where TEntity : Entity
    {
        ValidationResult Validate(TEntity entity, TValidate newValue);
    }
}
