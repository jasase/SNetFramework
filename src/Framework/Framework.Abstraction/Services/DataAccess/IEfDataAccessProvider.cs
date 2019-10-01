using System.Linq;

namespace Framework.Abstraction.Services.DataAccess
{
    public interface IEfDataAccessProvider : IDataAccessProvider
    {
        IQueryable<TEntity> Query<TEntity>()
            where TEntity : class;
    }
}
