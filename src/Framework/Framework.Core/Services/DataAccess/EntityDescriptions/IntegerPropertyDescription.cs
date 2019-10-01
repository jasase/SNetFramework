using Framework.Common.Services.Validator;
using Framework.Contracts.Entities;
using Framework.Contracts.Services;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;
using System;
using System.Linq.Expressions;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
{
    public class IntegerPropertyDescription<TEntity> : PropertyDescription<TEntity, int>, IIntegerPropertyDescription<TEntity>
        where TEntity : Entity
    {
        public IntegerPropertyDescription(Expression<Func<TEntity, int>> propertySelector, string displayName, IPropertyGroupDescription assignedGroup)
            : base(propertySelector, displayName, new EverythingValidValidator<int, TEntity>(), assignedGroup)
        { }

        public IntegerPropertyDescription(Expression<Func<TEntity, int>> propertySelector, string displayName, IValidator<int> validator, IPropertyGroupDescription assignedGroup)
            : base(propertySelector, displayName, validator, assignedGroup)
        { }

        public override void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public override void Accept(IPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public int GetIntegerValue(Entity value)
        {
            return GetIntegerValue(CheckAndCast(value));
        }

        public int GetIntegerValue(TEntity value)
        {
            return GetValueTyped(value);
        }

        public void SetIntegerValue(int value, Entity entity)
        {
            SetIntegerValue(value, CheckAndCast(entity));
        }

        public void SetIntegerValue(int value, TEntity entity)
        {
            SetValueTyped(entity, value);
        }
    }
}
