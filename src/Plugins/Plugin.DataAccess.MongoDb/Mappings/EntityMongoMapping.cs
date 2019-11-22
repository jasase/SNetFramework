using Framework.Abstraction.Entities;
using MongoDB.Bson.Serialization;

namespace Plugin.DataAccess.MongoDb.Mappings
{
    public class EntityMongoMapping : BsonClassMap<Entity>
    {
        public EntityMongoMapping()
        {
            MapIdProperty(x => x.Id);
            MapProperty(x => x.IsDeleted).SetDefaultValue(false);
        }
    }
}
