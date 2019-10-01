
using System;
using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
{
    public interface IEntityReferencePropertyDescription : IPropertyDescription
    {
        Type DestinationType { get; }
        IValidator<EntityReference> Validator { get; }
        EntityReference GetReferenceValue(Entity entity);
        void SetReferenceValue(EntityReference value, Entity entity);
        ValidationResult ValidateValue(Entity entity, EntityReference value);
    }

    public interface IEntityReferencePropertyDescription<TEntity> : IEntityReferencePropertyDescription, IPropertyDescription<TEntity>
        where TEntity : Entity
    {
        EntityReference GetReferenceValue(TEntity entity);
        void SetReferenceValue(EntityReference value, TEntity entity);
        ValidationResult ValidateValue(TEntity entity, EntityReference value);
    }
}
