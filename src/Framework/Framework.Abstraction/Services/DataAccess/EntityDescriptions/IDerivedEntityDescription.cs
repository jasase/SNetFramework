using Framework.Contracts.Entities;
using System;
using System.Collections.Generic;

namespace Framework.Contracts.Services.DataAccess.EntityDescriptions
{
    public interface IDerivedEntityDescription
    {
        string Name { get; }
        string DisplayName { get; }
        Type TypeOfEntity { get; }
        IEnumerable<IPropertyDescription> Properties { get; }
    }

    public interface IDerivedEntityDescription<TEntity> : IDerivedEntityDescription
        where TEntity : Entity
    {        
        new IEnumerable<IPropertyDescription<TEntity>> Properties { get; }
    }
}