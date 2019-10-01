using Framework.Abstraction.Entities;
using Framework.Abstraction.Helper;
using Framework.Abstraction.Services.DataAccess;
using System;
using System.Collections.Generic;

namespace Framework.Abstraction.Services
{
    public interface IManagerResolver
    {
        IEnumerable<IManager> GetAllManager();

        IMaybe<IManager<TEntity>> ResolveManager<TEntity>()
            where TEntity : Entity;

        IMaybe<IManager> ResolveManager(Type entity);
    }
}
