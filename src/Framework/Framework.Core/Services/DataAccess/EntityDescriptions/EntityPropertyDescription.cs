using System;
using System.Linq.Expressions;
using Framework.Contracts.Entities;
using Framework.Contracts.Services;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;

namespace Framework.Common.Services.DataAccess.EntityDescriptions
{
    public class EntityPropertyDescription<TEntity, TOtherEntity> :
        PropertyDescription<TEntity, TOtherEntity>,
        IEntityPropertyDescription<TEntity, TOtherEntity>
        where TEntity : Entity
        where TOtherEntity : Entity        
    {
        private readonly EntityDescription<TOtherEntity> _subEntity;

        public EntityPropertyDescription(EntityDescription<TOtherEntity> subEntity,
                                         Expression<Func<TEntity, TOtherEntity>> propertySelector,
                                         IPropertyGroupDescription assignedGroup) 
            : base(propertySelector, subEntity.DisplayName, CreateValidator(), assignedGroup)
        {
            _subEntity = subEntity;
        }

        public static IValidator<TOtherEntity> CreateValidator()
        {
            throw new NotImplementedException();
        }

        public IEntityDescription<TOtherEntity> SubEntity { get { return _subEntity; } }

        public override void Accept(IPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }

        public override void Accept(IGenericPropertyDescriptionVisitor visitor)
        {
            visitor.Handle(this);
        }
    }
}
