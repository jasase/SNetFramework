using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.IntegerValidators
{
    public abstract class IntegerValidator<TEntity> : BaseValidator<int, TEntity>
        where TEntity : Entity
    { }
}
