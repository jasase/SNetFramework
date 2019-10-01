using Framework.Contracts.Entities;

namespace Framework.Common.Services.Validator.StringValidators
{
    public class StringNotEmptyValidator<TEntity> : BaseStringValidator<TEntity>
        where TEntity : Entity
    {
        public StringNotEmptyValidator() : base(false)
        { }

    }
}
