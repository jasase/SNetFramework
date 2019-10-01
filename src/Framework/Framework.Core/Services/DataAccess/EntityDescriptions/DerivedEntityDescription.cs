using System;
using System.Collections.Generic;
using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using System.Linq;

namespace Framework.Core.Services.DataAccess.EntityDescriptions
{
    public class DerivedEntityDescription<TEntityBase, TEntityDerived> : IDerivedEntityDescription<TEntityBase>
        where TEntityBase : Entity
        where TEntityDerived : TEntityBase
    {
        private readonly string _displayName;
        private readonly DerivedPropertyDescription<TEntityBase, TEntityDerived>[] _properties;

        public DerivedEntityDescription(string displayName, IEnumerable<PropertyDescription<TEntityDerived>> properties)
        {
            _displayName = displayName;
            _properties = properties.Select(x => new DerivedPropertyDescription<TEntityBase, TEntityDerived>(x))
                                    .ToArray();
        }

        public string DisplayName { get { return _displayName; } }
        public string Name { get { return TypeOfEntity.Name; } }
        public IEnumerable<IPropertyDescription<TEntityBase>> Properties { get { return _properties; } }
        IEnumerable<IPropertyDescription> IDerivedEntityDescription.Properties { get { return Properties; } }
        public Type TypeOfEntity { get { return typeof(TEntityDerived); } }        
    }
}