using Framework.Contracts.Entities;
using System;

namespace Framework.Common.Services.Validator.DateTimeValidators
{
    public abstract class DateTimeValidator<TEntity> : BaseValidator<DateTime, TEntity>
        where TEntity : Entity
    {
    }
}
