using Framework.Contracts.Entities;
using Framework.Contracts.Helper;
using System;
using System.Collections.Generic;

namespace Framework.Contracts.Services.DataAccess
{
    public interface IDataAccessProvider
    {
        IEnumerable<T> GetAll<T>() where T : Entity;
        IEnumerable<T> GetAllDeleted<T>() where T : Entity;
        IMaybe<T> GetById<T>(Guid id) where T : Entity;
        IEnumerable<T> GetById<T>(Guid[] id) where T : Entity;
        bool Insert<T>(T data) where T : Entity;
        bool Update<T>(T data) where T : Entity;
        bool Delete<T>(Guid id) where T : Entity;
    }
}
