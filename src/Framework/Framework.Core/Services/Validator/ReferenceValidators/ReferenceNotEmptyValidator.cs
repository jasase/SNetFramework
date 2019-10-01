using Framework.Contracts.Entities;

namespace Framework.Common.Services.Validator.ReferenceValidators
{
    public class ReferenceNotEmptyValidator<TEntity> : BaseReferenceValidator<TEntity>
        where TEntity : Entity
    {
        public ReferenceNotEmptyValidator()
            : base(false)
        { }
    }
}
