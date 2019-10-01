using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;
using System;
using System.Collections.Generic;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public interface IEntityDescription : IBaseDescription
    {
        string Name { get; }
        string DisplayName { get; }
        Type TypeOfEntity { get; }
        IEnumerable<IPropertyGroupDescription> PropertyGroups { get; }
        IEnumerable<IPropertyDescription> Properties { get; }
        IEnumerable<IDerivedEntityDescription> DerivedEntities { get; }

        IEnumerable<IPropertyDescription> GetPropertiesOf(IPropertyGroupDescription group);
    }

    public interface IEntityDescription<TEntity> : IEntityDescription, IBaseDescription<TEntity>
        where TEntity : Entity
    {
        IEnumerable<IPropertyDescription<TEntity>> PropertiesGeneric { get; }
        IEnumerable<IDerivedEntityDescription<TEntity>> DerivedEntitiesGeneric { get; }
    }
}
