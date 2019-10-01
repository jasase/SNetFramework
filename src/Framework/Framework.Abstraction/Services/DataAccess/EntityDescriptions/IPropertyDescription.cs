using Framework.Contracts.Entities;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public interface IPropertyDescription : IBaseDescription
    {
        string Name { get; }
        string DisplayeName { get; }
        IPropertyGroupDescription Group { get; }

        object GetValue(object entity);
        void Accept(IPropertyDescriptionVisitor visitor);
        ValidationResult ValidateValue(Entity entity, object value);
    }

    public interface IPropertyDescription<in TEntity> : IPropertyDescription, IBaseDescription<TEntity>
        where TEntity : Entity
    {
        object GetValue(TEntity entity);
        void Accept(IGenericPropertyDescriptionVisitor visitor);
        ValidationResult ValidateValue(TEntity entity, object value);
    }
}
