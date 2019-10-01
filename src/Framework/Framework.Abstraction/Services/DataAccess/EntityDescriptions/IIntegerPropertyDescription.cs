using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
{
    public interface IIntegerPropertyDescription : IPropertyDescription
    {
        IValidator<int> Validator { get; }
        int GetIntegerValue(Entity value);
        void SetIntegerValue(int value, Entity entity);
    }

    public interface IIntegerPropertyDescription<TEntity> : IIntegerPropertyDescription, IPropertyDescription<TEntity>
        where TEntity : Entity
    {
        int GetIntegerValue(TEntity value);
        void SetIntegerValue(int value, TEntity entity);
        ValidationResult ValidateValue(TEntity entity, int value);
    }
}
