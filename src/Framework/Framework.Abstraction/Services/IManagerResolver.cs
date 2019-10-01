using Framework.Contracts.Entities;
using Framework.Contracts.Helper;
using Framework.Contracts.Services.DataAccess;
using System;
using System.Collections.Generic;

namespace Framework.Contracts.Services
{
    public interface IManagerResolver
    {
        IEnumerable<IManager> GetAllManager();

        IMaybe<IManager<TEntity>> ResolveManager<TEntity>()
            where TEntity : Entity;

        IMaybe<IManager> ResolveManager(Type entity);
    }
}
