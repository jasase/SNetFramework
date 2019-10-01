using Framework.Contracts.Entities;
using System;
using Framework.Contracts.Services;
using System.Linq.Expressions;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
{
    public class EntityReferencePropertyDescription<TEntity, TEntityReferenced>
        : PropertyDescription<TEntity, EntityReference>,
          IEntityReferencePropertyDescription<TEntity>
        where TEntity : Entity
        where TEntityReferenced : Entity
    {
        public EntityReferencePropertyDescription(Expression<Func<TEntity, EntityReference>> propertySelector,
                                                  string displayName,
                                                  IValidator<EntityReference> validator, 
                                                  IPropertyGroupDescription assignedGroup)
            : base(propertySelector, displayName, validator, assignedGroup)
        { }

        public Type DestinationType { get { return typeof(TEntityReferenced); } }

        public override void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public override void Accept(IPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public EntityReference GetReferenceValue(Entity entity)
        {
            return GetReferenceValue(CheckAndCast(entity));
        }

        public EntityReference GetReferenceValue(TEntity entity)
        {
            return GetValueTyped(entity);
        }

        public void SetReferenceValue(EntityReference value, Entity entity)
        {
            SetReferenceValue(value, CheckAndCast(entity));
        }

        public void SetReferenceValue(EntityReference value, TEntity entity)
        {
            SetValueTyped(entity, value);
        }
    }
}
