using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;
using System;
using System.Linq.Expressions;
using Framework.Contracts.Services;
using Framework.Common.Services.Validator.StringValidators;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
{
    public class StringPropertyDescription<TEntity> : PropertyDescription<TEntity, string>, IStringPropertyDescription<TEntity>
        where TEntity : Entity
    {
        private readonly IValidator<string> _validator;

        public StringPropertyDescription(Expression<Func<TEntity, string>> propertySelector, string displayName, IPropertyGroupDescription assignedGroup)
            : base(propertySelector, displayName, new StringNotEmptyValidator<TEntity>(), assignedGroup)
        { }

        public StringPropertyDescription(Expression<Func<TEntity, string>> propertySelector, string displayName, IValidator<string> validator, IPropertyGroupDescription assignedGroup)
            : base(propertySelector, displayName, validator, assignedGroup)
        { }        

        public override void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public override void Accept(IPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public string GetStringValue(Entity entity)
        {
            return GetValueTyped(CheckAndCast(entity));
        }

        public string GetStringValue(TEntity entity)
        {
            return GetValueTyped(entity);
        }

        public void SetStringValue(string value, Entity entity)
        {
            SetStringValue(value, CheckAndCast(entity));
        }

        public void SetStringValue(string value, TEntity entity)
        {
            SetValueTyped(entity, value);
        }
    }
}
