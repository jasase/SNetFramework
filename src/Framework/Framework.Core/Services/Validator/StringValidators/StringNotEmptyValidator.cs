using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.StringValidators
{
    public class StringNotEmptyValidator<TEntity> : BaseStringValidator<TEntity>
        where TEntity : Entity
    {
        public StringNotEmptyValidator() : base(false)
        { }

    }
}
