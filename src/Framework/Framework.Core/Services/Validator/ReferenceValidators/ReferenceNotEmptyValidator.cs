using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.ReferenceValidators
{
    public class ReferenceNotEmptyValidator<TEntity> : BaseReferenceValidator<TEntity>
        where TEntity : Entity
    {
        public ReferenceNotEmptyValidator()
            : base(false)
        { }
    }
}
