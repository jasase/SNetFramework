using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections;
using Framework.Contracts.Helper;
using System.Linq;
using Framework.Common.Helper;

namespace Framework.Common.Services.DataAccess
{
    public abstract class EntityRepositoryWithDto<TEntity, TDto> : IRepository<TEntity>
        where TEntity : Entity
        where TDto : Entity
    {
        private readonly IDataAccessProvider _dataAccessProvider;

        public EntityRepositoryWithDto(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        public bool Delete(Guid id)
            => _dataAccessProvider.Delete<TDto>(id);

        public virtual IEnumerable<TEntity> GetAll()
            => _dataAccessProvider.GetAll<TDto>().Select(x => MapToEntity(x));

        public virtual IEnumerable<TEntity> GetAllDeleted()
            => _dataAccessProvider.GetAllDeleted<TDto>().Select(x => MapToEntity(x));

        public IEnumerable GetAllAsObjects()
            => GetAll();

        public virtual IEnumerable<TEntity> GetById(Guid[] id)
            => _dataAccessProvider.GetById<TDto>(id).Select(x => MapToEntity(x));

        public virtual IMaybe<TEntity> GetById(Guid id)
        {
            var value = _dataAccessProvider.GetById<TDto>(id);
            if (value.HasValue)
            {
                return new Maybe<TEntity>(MapToEntity(value.Value));
            }
            return new Maybe<TEntity>(null);
        }

        public IMaybe<object> GetByIdAsObject(Guid id)
            => GetById(id);

        public virtual bool Insert(TEntity data)
            => _dataAccessProvider.Insert(MapToDto(data));

        public virtual bool Update(TEntity data)
            => _dataAccessProvider.Update(MapToDto(data));

        protected abstract TDto MapToDto(TEntity entity);

        protected abstract TEntity MapToEntity(TDto entity);
    }
}
