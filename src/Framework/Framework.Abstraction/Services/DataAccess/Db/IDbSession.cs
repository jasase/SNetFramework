using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.Db
{
  public interface IDbSession
  {
    T Get<T>(Guid id) where T : Entity;

    void Save<T>(T entity) where T : Entity;

    IEnumerable<T> GetAll<T>() where T : Entity;

    IQueryable<T> Query<T>() where T : Entity;
  }
}
