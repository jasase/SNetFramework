using Framework.Core.Services.DataAccess;
using Framework.Abstraction.Entities;
using MongoDB.Driver;
using Framework.Contracts.Services.DataAccess;

namespace Plugin.DataAccess.MongoDb
{
    public abstract class EntityMongoRepository<TEntity> : EntityRepository<TEntity>
        where TEntity : Entity
    {
        public IMongoCollection<TEntity> Collection { get; }

        public EntityMongoRepository(IMongoDataAccessProvider dataAccessProvider)
            : base(dataAccessProvider)
        {
            Collection = dataAccessProvider.MongoFactory.GetOrCreateCollection<TEntity>();
        }

        protected FilterDefinitionBuilder<TEntity> Filter() => Builders<TEntity>.Filter;
        protected FilterDefinitionBuilder<TDerivedEntity> Filter<TDerivedEntity>()
            where TDerivedEntity : TEntity
            => Builders<TDerivedEntity>.Filter;

        protected UpdateDefinitionBuilder<TEntity> Update() => Builders<TEntity>.Update;
        protected UpdateDefinitionBuilder<TDerivedEntity> Update<TDerivedEntity>()
            where TDerivedEntity : TEntity
            => Builders<TDerivedEntity>.Update;

    }
}
