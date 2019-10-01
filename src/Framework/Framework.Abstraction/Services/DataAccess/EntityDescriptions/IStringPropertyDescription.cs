using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
{
    public interface IStringPropertyDescription : IPropertyDescription
    {
        IValidator<string> Validator { get; }
        string GetStringValue(Entity entity);
        void SetStringValue(string value, Entity entity);

        ValidationResult ValidateValue(Entity entity, string value);
    }

    public interface IStringPropertyDescription<TEntity> : IStringPropertyDescription, IPropertyDescription<TEntity>
        where TEntity : Entity
    {
        string GetStringValue(TEntity entity);
        void SetStringValue(string value, TEntity entity);
        ValidationResult ValidateValue(TEntity entity, string value);
    }
}
