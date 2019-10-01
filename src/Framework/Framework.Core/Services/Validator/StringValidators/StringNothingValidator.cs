using Framework.Contracts.Entities;

namespace Framework.Common.Services.Validator.StringValidators
{
    public class StringNothingValidator<TEntity> : BaseStringValidator<TEntity>
        where TEntity : Entity
    {
        public StringNothingValidator() : base(true)
        { }
    }
}
