using Framework.Contracts.Entities;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public interface IBaseDescription
    {
        ValidationResult Validate(Entity entity);        
    }

    public interface IBaseDescription<in TEntity>
        where TEntity : Entity
    {
        ValidationResult Validate(TEntity entity);
    }
}
