﻿using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.Validator.StringValidators
{
    public abstract class BaseStringValidator<TEntity> : BaseValidator<string, TEntity>
        where TEntity : Entity
    {
        private bool _allowEmpty;

        public BaseStringValidator(bool allowEmpty)
        {
            _allowEmpty = allowEmpty;
        }


        public override ValidationResult Validate(TEntity entity, string newValue)
        {
            if (!_allowEmpty && string.IsNullOrEmpty(newValue))
            {
                return ValidationResult.Invalid("Eine leere Zeichenfolge ist nicht erlaubt");
            }
            return ValidationResult.Valid();
        }        
    }
}
