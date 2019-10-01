using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess;

namespace Framework.Core.Services.DataAccess
{
    public abstract class EntityRepository<TEntity> : EntityRepositoryWithDto<TEntity, TEntity>
        where TEntity : Entity
    {
        public EntityRepository(IDataAccessProvider dataAccessProvider) 
            : base(dataAccessProvider)
        { }

        protected override TEntity MapToDto(TEntity entity)
        {
            return entity;
        }

        protected override TEntity MapToEntity(TEntity entity)
        {
            return entity;
        }
    }
}
