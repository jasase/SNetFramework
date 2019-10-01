using Framework.Contracts.Entities;

namespace Framework.Common.Services.Validator.IntegerValidators
{
    public abstract class IntegerValidator<TEntity> : BaseValidator<int, TEntity>
        where TEntity : Entity
    { }
}
