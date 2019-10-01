using Framework.Abstraction.Entities;
using System;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
{
    public interface IDateTimePropertyDescription : IPropertyDescription
    {
        IValidator<DateTime> Validator { get; }
        DateTime GetDateTimeValue(Entity entity);
        void SetDateTimeValue(DateTime value, Entity entity);        
    }

    public interface IDateTimePropertyDescription<TEntity> : IDateTimePropertyDescription, IPropertyDescription<TEntity>
        where TEntity : Entity
    {
        DateTime GetDateTimeValue(TEntity entity);
        void SetDateTimeValue(DateTime value, TEntity entity);
        ValidationResult ValidateValue(TEntity entity, DateTime value);
    }
}
