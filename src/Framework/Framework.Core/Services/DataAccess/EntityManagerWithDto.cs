using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess;
using System;
using System.Collections.Generic;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Messages.EntityMessages;
using Framework.Abstraction.Helper;
using Framework.Abstraction.Services.DataAccess.EntityDescriptions;
using Framework.Core.Services.DataAccess.EntityDescriptions;
using System.Linq;
using Framework.Core.Helper;

namespace Framework.Core.Services.DataAccess
{
    public abstract class EntityManagerWithDto<TEntity, TDto> : IManager<TEntity>
        where TEntity : Entity
        where TDto : Entity
    {
        private readonly IRepository<TDto> _repository;
        private EntityDescription<TEntity> _description;

        protected IEventService EventService { get; }

        public EntityManagerWithDto(IRepository<TDto> repository, IEventService eventService)
        {
            _repository = repository;
            EventService = eventService;
        }

        protected abstract EntityDescription<TEntity> CreateEntityDescription();

        public IEntityDescription Description => EntityDescription;
        public IEntityDescription<TEntity> DescriptionGeneric => EntityDescription;
        public EntityDescription<TEntity> EntityDescription
        {
            get
            {
                if (_description == null)
                {
                    _description = CreateEntityDescription();
                }
                return _description;
            }
        }

        public virtual bool Delete(Guid id)
        {
            var result = _repository.Delete(id);
            if (result) EventService.Publish(new EntityDeletedMessage(typeof(TEntity), id));
            return result;
        }

        public virtual IEnumerable<TEntity> GetAll()
            => _repository.GetAll().Select(x => MapToEntity(x));

        public virtual IEnumerable<TEntity> GetAllDeleted()
            => _repository.GetAllDeleted().Select(x => MapToEntity(x));        

        public IEnumerable<Entity> GetAllAsEntity()
            => GetAll();

        public IMaybe<TEntity> GetById(Guid id)
        {
            var value = _repository.GetById(id);
            if (value.HasValue)
            {
                return new Maybe<TEntity>(MapToEntity(value.Value));
            }
            return new Maybe<TEntity>(null);
        }

        public IMaybe<Entity> GetByIdAsEntity(Guid id)
            => GetById(id);

        public virtual bool Insert(TEntity data)
        {
            ProcessEntityBeforeWrite(data);
            var result = _repository.Insert(MapToDto(data));
            if (result) EventService.Publish(new EntityInsertedMessage(typeof(TEntity), data.Id));
            return result;
        }

        public virtual bool Update(TEntity data)
        {
            ProcessEntityBeforeWrite(data);
            var result = _repository.Update(MapToDto(data));
            if (result) EventService.Publish(new EntityUpdatedMessage(typeof(TEntity), data.Id));
            return result;
        }

        public bool UpdateAsEntity(Entity id)
        {
            var typedEntity = TryCast(id);
            return Update(typedEntity);
        }

        public bool InsertAsEntity(Entity id)
        {
            var typedEntity = TryCast(id);
            return Insert(typedEntity);
        }

        protected abstract TDto MapToDto(TEntity entity);

        protected abstract TEntity MapToEntity(TDto entity);

        private TEntity TryCast(Entity entiy)
        {
            var typedEntity = entiy as TEntity;
            if (typedEntity == null)
                throw new InvalidOperationException(string.Format(
                    "Manager '{0}' only supports entities of type '{1}' but provided entity was of type '{2}'",
                    GetType().FullName,
                    typeof(TEntity).FullName,
                    entiy.GetType().FullName));

            return typedEntity;
        }

        private void ProcessEntityBeforeWrite(TEntity entity)
        {
            var visitor = new ProcessEntityBeforeWriteVisitor(Description, entity);
            foreach (var property in DescriptionGeneric.PropertiesGeneric)
            {
                property.Accept(visitor);
            }
        }

        

        class ProcessEntityBeforeWriteVisitor : IGenericPropertyDescriptionVisitor
        {
            private readonly Entity _entity;
            private readonly IEntityDescription _entityDescription;

            public ProcessEntityBeforeWriteVisitor(IEntityDescription entityDescription, Entity entity)
            {
                _entityDescription = entityDescription;
                _entity = entity;
            }

            public void Handle<TEntity1>(IDateTimePropertyDescription<TEntity1> integerPropertyDescription) where TEntity1 : Entity
            { }

            public void Handle<TEntity1>(IBoolPropertyDescription<TEntity1> propertyDescription) where TEntity1 : Entity
            { }

            public void Handle<TEntity1>(IEntityReferencePropertyDescription<TEntity1> propertyDescription) where TEntity1 : Entity
            {
                var reference = propertyDescription.GetReferenceValue(_entity);
                reference.TargetType = propertyDescription.DestinationType.FullName;
                reference.SourceType = _entityDescription.TypeOfEntity.FullName;
                reference.SourceId = _entity.Id;
            }

            public void Handle<TEntity1>(IIntegerPropertyDescription<TEntity1> integerPropertyDescription) where TEntity1 : Entity
            { }

            public void Handle<TEntity1>(IStringPropertyDescription<TEntity1> integerPropertyDescription) where TEntity1 : Entity
            { }

            public void Handle<TEntity1, TOtherEntity>(IEntityPropertyDescription<TEntity1, TOtherEntity> entityPropertyDescription)
                where TEntity1 : Entity
                where TOtherEntity : Entity
            { }
        }
    }
}
