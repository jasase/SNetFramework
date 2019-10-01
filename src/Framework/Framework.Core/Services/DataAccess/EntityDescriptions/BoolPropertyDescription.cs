using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using System;
using Framework.Abstraction.Services;
using System.Linq.Expressions;

namespace Framework.Core.Services.DataAccess.EntityDescriptions
{
    public class BoolPropertyDescription<TEntity> : PropertyDescription<TEntity, bool>,
                                                    IBoolPropertyDescription<TEntity>
        where TEntity : Entity
    {
        public BoolPropertyDescription(Expression<Func<TEntity, bool>> propertySelector, 
                                       string displayName, 
                                       IValidator<bool> validator,
                                       IPropertyGroupDescription assignedGroup) 
            : base(propertySelector, displayName, validator, assignedGroup)
        { }
      
        public bool GetBoolValue(Entity entity)
        {
            return GetBoolValue(CheckAndCast(entity));
        }

        public bool GetBoolValue(TEntity entity)
        {
            return GetValueTyped(entity);
        }

        public void SetBoolValue(bool value, Entity entity)
        {
            SetBoolValue(value, CheckAndCast(entity));
        }

        public void SetBoolValue(bool value, TEntity entity)
        {
            SetValueTyped(entity, value);
        }

        public override void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public override void Accept(IPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }
    }
}
