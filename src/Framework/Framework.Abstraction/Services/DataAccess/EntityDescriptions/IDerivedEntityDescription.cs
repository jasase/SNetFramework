using Framework.Abstraction.Entities;
using System;
using System.Collections.Generic;

namespace Framework.Abstraction.Services.DataAccess.EntityDescriptions
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