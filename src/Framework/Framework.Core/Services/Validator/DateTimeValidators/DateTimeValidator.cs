using Framework.Abstraction.Entities;
using System;

namespace Framework.Core.Services.Validator.DateTimeValidators
{
    public abstract class DateTimeValidator<TEntity> : BaseValidator<DateTime, TEntity>
        where TEntity : Entity
    {
    }
}
