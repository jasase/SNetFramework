using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.BoolValidators
{
    public abstract class BaseBoolValidator<TEntity> : BaseValidator<bool, TEntity>
        where TEntity : Entity
    {
    }
}
