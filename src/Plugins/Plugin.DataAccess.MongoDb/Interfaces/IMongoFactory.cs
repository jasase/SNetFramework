using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Plugin.DataAccess.MongoDb.Interfaces
{
    public interface IMongoFactory
    {
        IMongoCollection<T> GetOrCreateCollection<T>();
        IMongoCollection<BsonDocument> GetOrCreateCollectionBson<T>();
        GridFSBucket<Guid> GetGridFsBucket();
    }
}
