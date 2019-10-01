using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
{
    public interface IEntityPropertyDescription<TOtherEntity> : IPropertyDescription
        where TOtherEntity : Entity
    {
        IEntityDescription<TOtherEntity> SubEntity { get; }
    }

    public interface IEntityPropertyDescription<TEntity, TOtherEntity> 
        : IEntityPropertyDescription<TOtherEntity>, IPropertyDescription<TEntity>
        where TEntity : Entity
        where TOtherEntity : Entity
    { }
}
