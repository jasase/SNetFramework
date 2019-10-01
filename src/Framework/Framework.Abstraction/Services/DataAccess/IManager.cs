using Framework.Contracts.Entities;
using Framework.Contracts.Helper;
using Framework.Contracts.Services.DataAccess.EntityDescriptions;
using System;
using System.Collections.Generic;

namespace Framework.Contracts.Services.DataAccess
{
    public interface IManager
    {
        IEntityDescription Description { get; }
        IEnumerable<Entity> GetAllAsEntity();
        IMaybe<Entity> GetByIdAsEntity(Guid id);
        bool UpdateAsEntity(Entity id);
        bool InsertAsEntity(Entity id);
        bool Delete(Guid id);
    }

    public interface IManager<T> : IManager where T : Entity
    {
        IEntityDescription<T> DescriptionGeneric { get; }
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllDeleted();
        IMaybe<T> GetById(Guid id);
        bool Insert(T data);
        bool Update(T data);        
    }
}
