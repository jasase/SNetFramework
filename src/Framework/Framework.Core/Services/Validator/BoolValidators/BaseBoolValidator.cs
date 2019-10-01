using Framework.Contracts.Entities;

namespace Framework.Common.Services.Validator.BoolValidators
{
    public abstract class BaseBoolValidator<TEntity> : BaseValidator<bool, TEntity>
        where TEntity : Entity
    {
    }
}
