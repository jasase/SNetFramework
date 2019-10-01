using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;
using System;
using Framework.Contracts.Services;
using System.Linq.Expressions;
using Framework.Common.Services.Validator.DateTimeValidators;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
{
    public class DateTimePropertyDescription<TEntity> : PropertyDescription<TEntity, DateTime>, IDateTimePropertyDescription<TEntity>
        where TEntity : Entity
    {
        public DateTimePropertyDescription(Expression<Func<TEntity, DateTime>> propertySelector, string displayName, IValidator<DateTime> validator, PropertyGroupDescription<TEntity> assignedGroup)
            : base(propertySelector, displayName, validator, assignedGroup)
        { }

        public DateTimePropertyDescription(Expression<Func<TEntity, DateTime>> propertySelector, string displayName, PropertyGroupDescription<TEntity> assignedGroup)
           : base(propertySelector, displayName, new DateTimeNotMinNorMaxValueValidator<TEntity>(), assignedGroup)
        { }

        public override void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public override void Accept(IPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public DateTime GetDateTimeValue(Entity entity)
        {
            return GetDateTimeValue(CheckAndCast(entity));
        }

        public DateTime GetDateTimeValue(TEntity entity)
        {
            return GetValueTyped(entity);
        }

        public void SetDateTimeValue(DateTime value, Entity entity)
        {
            SetDateTimeValue(value, CheckAndCast(entity));
        }

        public void SetDateTimeValue(DateTime value, TEntity entity)
        {
            SetValueTyped(entity, value);
        }

        public ValidationResult ValidateValue(Entity entity, DateTime value)
        {
            throw new NotImplementedException();
        }
    }
}
