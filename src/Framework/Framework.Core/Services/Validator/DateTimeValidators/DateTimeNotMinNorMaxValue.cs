﻿using System;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using Framework.Abstraction.Entities;

namespace Framework.Core.Services.Validator.DateTimeValidators
{
    public class DateTimeNotMinNorMaxValueValidator<TEntity> : DateTimeValidator<TEntity>
                where TEntity : Entity
    {
        public override ValidationResult Validate(TEntity entity, DateTime validate)
        {
            if (validate == DateTime.MinValue || validate == DateTime.MaxValue)
            {
                return ValidationResult.Invalid("Es ist kein Datum gesetzt.");
            }
            return ValidationResult.Valid();
        }
    }
}
