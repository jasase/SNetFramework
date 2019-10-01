using Framework.Abstraction.Entities;
using Framework.Abstraction.Helper;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Abstraction.Services.DataAccess
{
    public interface IRepository
    {
        IEnumerable GetAllAsObjects();
        IMaybe<object> GetByIdAsObject(Guid id);
    }

    public interface IRepository<T> : IRepository where T : Entity
    {        
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllDeleted();
        IEnumerable<T> GetById(Guid[] id);
        IMaybe<T> GetById(Guid id);
        bool Insert(T data);
        bool Update(T data);
        bool Delete(Guid id);
    }
}
