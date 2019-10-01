using System;
using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using System.Linq.Expressions;
using Framework.Abstraction.Services;
using Framework.Core.Helper;

namespace Framework.Core.Services.DataAccess.EntityDescriptions
{
    public abstract class PropertyDescription<TEntity> : IPropertyDescription<TEntity>
        where TEntity : Entity
    {
        private readonly string _displayName;
        private readonly IPropertyGroupDescription _group;

        public string DisplayeName { get { return _displayName; } }
        public abstract string Name { get; }

        public IPropertyGroupDescription Group { get { return _group; } }
        IPropertyGroupDescription IPropertyDescription.Group { get { return Group; } }

        public PropertyDescription(string displayName, IPropertyGroupDescription assignedGroup)
        {
            _displayName = displayName;
            _group = assignedGroup;
        }

        public object GetValue(object entity)
        {
            return GetValue(CheckAndCast(entity));
        }

        public abstract object GetValue(TEntity entity);
        public abstract void Accept(IPropertyDescriptionVisitor visitor);
        public abstract void Accept(IGenericPropertyDescriptionVisitor visitor);

        public TEntity CheckAndCast(object entity)
        {
            if (entity is TEntity)
            {
                return (TEntity)entity;
            }
            throw new ArgumentException(string.Format("Provided type '{0}' is not an entity of type '{1}'", entity.GetType(), typeof(TEntity)), "entity");
        }

        public ValidationResult ValidateAndCast(object entity, Func<TEntity,ValidationResult> validate)
        {
            if (entity is TEntity)
            {
                return validate( (TEntity)entity);
            }
            return ValidationResult.Invalid("Wrong type");            
        }

        public ValidationResult Validate(Entity entity)
        {
            return ValidateAndCast(entity, e => Validate(e));
        }
        public ValidationResult ValidateValue(Entity entity, object value)
        {
            return ValidateAndCast(entity, e => ValidateValue(e, value));
        }

        public abstract ValidationResult Validate(TEntity entity);
        public abstract ValidationResult ValidateValue(TEntity entity, object value);
    }

    public abstract class PropertyDescription<TEntity, TData> : PropertyDescription<TEntity>
        where TEntity : Entity
    {
        private Func<TEntity, TData> _getter;
        private Action<TEntity, TData> _setter;
        private readonly string _name;

        public override string Name { get { return _name; } }
        public IValidator<TData> Validator { get; private set; }

        public PropertyDescription(Expression<Func<TEntity, TData>> propertySelector,
                                   string displayName,
                                   IValidator<TData> validator,
                                   IPropertyGroupDescription assignedGroup) : base(displayName, assignedGroup)
        {
            var memberExpression = ExpressionHelper.GetMemberExpression(propertySelector);
            _getter = ExpressionHelper.DetermineGetter<TEntity, TData>(memberExpression);
            _setter = ExpressionHelper.DetermineSetter<TEntity, TData>(memberExpression);
            _name = memberExpression.Member.Name;
            Validator = validator;
        }

        public override object GetValue(TEntity entity)
        {
            return GetValueTyped(entity);
        }

        public override ValidationResult Validate(TEntity entity)
        {
            return Validator.Validate(entity, _getter(entity));
        }

        public ValidationResult ValidateValue(Entity entity, TData data)
        {
            return Validator.Validate(entity, data);
        }
        public ValidationResult ValidateValue(TEntity entity, TData data)
        {
            return Validator.Validate(entity, data);
        }

        public override ValidationResult ValidateValue(TEntity entity, object value)
        {
            if (value is TData || value == null)
            {
                return ValidateValue(entity, (TData)value);
            }

            return ValidationResult.Invalid(string.Format("Provided type '{0}' is not an entity of type '{1}'", value.GetType(), typeof(TData)));
        }

        protected TData GetValueTyped(TEntity entity)
        {
            return _getter(entity);
        }

        protected void SetValueTyped(TEntity entity, TData value)
        {
            _setter(entity, value);
        }
    }
}