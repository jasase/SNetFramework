using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.IntegerValidators
{
    public class PositiveIntegerValidator<TEntity> : IntegerValidator<TEntity>
                where TEntity : Entity
    {
        private readonly bool _excludeZero;

        public PositiveIntegerValidator(bool excludeZero)
        {
            _excludeZero = excludeZero;
        }

        public override ValidationResult Validate(TEntity entity, int validate)
        {
            if (_excludeZero && validate < 1)
            {
                return ValidationResult.Invalid("Die Zahl darf nicht kleiner als 1 sein.");
            }

            if (!_excludeZero && validate < 0)
            {
                return ValidationResult.Invalid("Die Zahl darf nicht kleiner als 0 sein.");
            }
            return ValidationResult.Valid();
        }
    }
}
