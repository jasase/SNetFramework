using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
{
    public class EntityDescription<TEntity> : IEntityDescription<TEntity>
        where TEntity : Entity
    {
        private readonly string _displayName;
        private readonly string _name;
        private readonly PropertyGroupDescription<TEntity>[] _propertyGroups;
        private readonly PropertyDescription<TEntity>[] _properties;
        private readonly IDerivedEntityDescription<TEntity>[] _derivedEntities;

        public EntityDescription(string displayName,
                                 IEnumerable<PropertyGroupDescription<TEntity>> propertyGroups,
                                 IEnumerable<PropertyDescription<TEntity>> properties)
            : this(displayName, propertyGroups, properties, new IDerivedEntityDescription<TEntity>[0])
        { }

        public EntityDescription(string displayName,
                                 IEnumerable<PropertyGroupDescription<TEntity>> propertyGroups,
                                 IEnumerable<PropertyDescription<TEntity>> properties,
                                 IEnumerable<IDerivedEntityDescription<TEntity>> derivedEntities)
        {
            _displayName = displayName;
            _name = typeof(TEntity).Name;
            _propertyGroups = propertyGroups.ToArray();
            _properties = properties.ToArray();
            _derivedEntities = derivedEntities.ToArray();

            CheckProperties();
        }

        private void CheckProperties()
        {
            var firstFail = _properties.FirstOrDefault(x => !_propertyGroups.Contains(x.Group));
            if (firstFail != null)
            {
                throw new ArgumentException(string.Format("Provided group [{0}] for property [{1}] does not belong to entity [{2}",
                                                          firstFail.Group.Name,
                                                          firstFail.Name,
                                                          Name));
            }
        }

        public string Name { get { return _name; } }
        public string DisplayName { get { return _displayName; } }
        public Type TypeOfEntity { get { return typeof(TEntity); } }

        public IEnumerable<IPropertyDescription> Properties { get { return _properties; } }
        public IEnumerable<IPropertyDescription<TEntity>> PropertiesGeneric { get { return _properties; } }
        public IEnumerable<IPropertyGroupDescription> PropertyGroups { get { return _propertyGroups; } }
        public IEnumerable<IDerivedEntityDescription<TEntity>> DerivedEntitiesGeneric { get { return _derivedEntities; } }
        public IEnumerable<IDerivedEntityDescription> DerivedEntities { get { return _derivedEntities; } }

        public ValidationResult Validate(Entity entity)
        {
            foreach (var property in Properties)
            {
                var result = property.Validate(entity);
                if (!result.IsValid) return result;
            }

            return ValidationResult.Valid();
        }

        public ValidationResult Validate(TEntity entity)
        {
            foreach (var property in Properties)
            {
                var result = property.Validate(entity);
                if (!result.IsValid) return result;
            }

            return ValidationResult.Valid();
        }

        public IEnumerable<IPropertyDescription<TEntity>> GetPropertiesOf(IPropertyGroupDescription group)
        {
            return _properties.Where(x => x.Group.Equals(group));
        }
        IEnumerable<IPropertyDescription> IEntityDescription.GetPropertiesOf(IPropertyGroupDescription group)
        {
            return GetPropertiesOf(group);
        }

        protected TEntity CheckAndCast(object entity)
        {
            if (entity is TEntity)
            {
                return (TEntity)entity;
            }
            throw new ArgumentException(string.Format("Provided type '{0}' is not an entity of type '{1}'", entity.GetType(), typeof(TEntity)), "entity");
        }
    }
}
