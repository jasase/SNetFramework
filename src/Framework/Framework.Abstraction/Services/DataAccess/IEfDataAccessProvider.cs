using System.Linq;

namespace Framework.Contracts.Services.DataAccess
{
    public interface IEfDataAccessProvider : IDataAccessProvider
    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class;
    }
}
