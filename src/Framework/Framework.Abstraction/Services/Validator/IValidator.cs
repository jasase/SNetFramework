using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Contracts.Services
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
