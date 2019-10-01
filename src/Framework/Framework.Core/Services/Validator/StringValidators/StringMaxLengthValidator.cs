using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.Validator.StringValidators
{
    public class StringMaxLengthValidator<TEntity> : BaseStringValidator<TEntity>
        where TEntity : Entity
    {
        private readonly int _maxLength;

        public StringMaxLengthValidator(int maxLength)
            : base(false)
        {
            _maxLength = maxLength;
        }

        public override ValidationResult Validate(TEntity entity, string newValue)
        {
            var baseResult = base.Validate(entity, newValue);
            if (baseResult.IsValid)
            {
                if (newValue.Length > _maxLength)
                {
                    return ValidationResult.Invalid(string.Format("Maximal erlaubte Länge von {0} Zeichen überschritten", _maxLength));
                }
                return ValidationResult.Valid();
            }
            return baseResult;
        }
    }
}
