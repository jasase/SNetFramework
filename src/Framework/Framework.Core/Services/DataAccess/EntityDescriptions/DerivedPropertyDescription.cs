using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;

namespace Framework.Core.Services.DataAccess.EntityDescriptions
{
    public class DerivedPropertyDescription<TEntityBase, TEntityDerived> : IPropertyDescription<TEntityBase>
        where TEntityBase : Entity
        where TEntityDerived : TEntityBase
    {
        private readonly PropertyDescription<TEntityDerived> _propertyDescription;

        public string DisplayeName { get { return _propertyDescription.DisplayeName; } }
        public IPropertyGroupDescription Group { get { return _propertyDescription.Group; } }
        public string Name { get { return _propertyDescription.Name; } }

        public DerivedPropertyDescription(PropertyDescription<TEntityDerived> propertyDescription)
        {
            _propertyDescription = propertyDescription;
        }

        public void Accept(IPropertyDescriptionVisitor visitor)
        {
            _propertyDescription.Accept(visitor);
        }
        public void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            _propertyDescription.Accept(visitor);
        }

        public object GetValue(object entity)
        {
            return _propertyDescription.GetValue(entity);
        }

        public object GetValue(TEntityBase entity)
        {
            return _propertyDescription.GetValue(entity);
        }

        public ValidationResult Validate(TEntityBase entity)
        {
            return _propertyDescription.Validate(entity);
        }

        public ValidationResult Validate(Entity entity)
        {
            return _propertyDescription.Validate(entity);
        }

        public ValidationResult ValidateValue(Entity entity, object value)
        {
            return _propertyDescription.ValidateValue(entity, value);
        }

        public ValidationResult ValidateValue(TEntityBase entity, object value)
        {
            return _propertyDescription.ValidateValue(entity, value);
        }
    }
}
