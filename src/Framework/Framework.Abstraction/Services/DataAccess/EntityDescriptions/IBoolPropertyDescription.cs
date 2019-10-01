using Framework.Contracts.Entities;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public interface IBoolPropertyDescription : IPropertyDescription
    {
        IValidator<bool> Validator { get; }
        bool GetBoolValue(Entity entity);
        void SetBoolValue(bool value, Entity entity);
        ValidationResult ValidateValue(Entity entity, bool value);
    }

    public interface IBoolPropertyDescription<TEntity> : IBoolPropertyDescription, IPropertyDescription<TEntity>
        where TEntity : Entity
    {
        bool GetBoolValue(TEntity entity);
        void SetBoolValue(bool value, TEntity entity);
        ValidationResult ValidateValue(TEntity entity, bool value);
    }
}
