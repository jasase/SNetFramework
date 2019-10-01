using Framework.Abstraction.Entities;
using Framework.Abstraction.Services.DataAccess;
using Framework.Abstraction.Extension;

namespace Framework.Core.Services.DataAccess
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
