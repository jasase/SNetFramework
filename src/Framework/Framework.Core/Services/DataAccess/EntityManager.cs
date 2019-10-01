using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess;
using Framework.Contracts.Extension;

namespace Framework.Common.Services.DataAccess
{
    public abstract class EntityManager<TEntity> : EntityManagerWithDto<TEntity, TEntity>
        where TEntity : Entity
    {
        public EntityManager(IRepository<TEntity> repository, IEventService eventService)
            : base(repository, eventService)
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
