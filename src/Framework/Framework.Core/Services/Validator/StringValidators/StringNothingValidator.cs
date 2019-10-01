using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.StringValidators
{
    public class StringNothingValidator<TEntity> : BaseStringValidator<TEntity>
        where TEntity : Entity
    {
        public StringNothingValidator() : base(true)
        { }
    }
}
