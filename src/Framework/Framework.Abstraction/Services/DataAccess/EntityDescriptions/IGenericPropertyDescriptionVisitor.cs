using Framework.Contracts.Entities;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public interface IGenericPropertyDescriptionVisitor
    {
        void Handle<TEntity>(IStringPropertyDescription<TEntity> integerPropertyDescription) where TEntity : Entity;
        void Handle<TEntity>(IIntegerPropertyDescription<TEntity> integerPropertyDescription) where TEntity : Entity;
        void Handle<TEntity>(IDateTimePropertyDescription<TEntity> integerPropertyDescription) where TEntity : Entity;
        void Handle<TEntity>(IEntityReferencePropertyDescription<TEntity> propertyDescription) where TEntity : Entity;
        void Handle<TEntity>(IBoolPropertyDescription<TEntity> propertyDescription) where TEntity : Entity;
        void Handle<TEntity, TOtherEntity>(IEntityPropertyDescription<TEntity, TOtherEntity> entityPropertyDescription)
            where TEntity : Entity
            where TOtherEntity : Entity;
    }
}