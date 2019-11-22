using System;
using System.Collections.Generic;
using Framework.Core.Helper;
using Framework.Abstraction.Entities;
using Framework.Abstraction.Helper;
using Framework.Contracts.Services.DataAccess;
using Plugin.DataAccess.MongoDb.Interfaces;
using MongoDB.Driver;

namespace Plugin.DataAccess.MongoDb
{
    public class MongoDataAccessProvider : IMongoDataAccessProvider
    {
        public IMongoFactory MongoFactory { get; }

        public MongoDataAccessProvider(IMongoFactory mongoFactory)
        {
            MongoFactory = mongoFactory;
        }

        public bool Delete<T>(Guid id) where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            var result = collection.UpdateOne(Builders<T>.Filter.Eq(x => x.Id, id),
                                              Builders<T>.Update.Set(x => x.IsDeleted, true));
            return result.ModifiedCount > 0;
        }

        public IEnumerable<T> GetAll<T>() where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            return collection.Find(Builders<T>.Filter.Eq(x => x.IsDeleted, false) | !Builders<T>.Filter.Exists(x => x.IsDeleted)).ToEnumerable();
        }

        public IEnumerable<T> GetAllDeleted<T>() where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            return collection.Find(Builders<T>.Filter.Eq(x => x.IsDeleted, true)).ToEnumerable();
        }

        public IMaybe<T> GetById<T>(Guid id) where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            var result = collection.Find(Builders<T>.Filter.Eq(x => x.Id, id) &
                                         (Builders<T>.Filter.Eq(x => x.IsDeleted, false) | !Builders<T>.Filter.Exists(x => x.IsDeleted))).FirstOrDefault();
            return new Maybe<T>(result);
        }

        public IEnumerable<T> GetById<T>(Guid[] id) where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            return collection.Find(Builders<T>.Filter.In(x => x.Id, id) &
                                   (Builders<T>.Filter.Eq(x => x.IsDeleted, false) | !Builders<T>.Filter.Exists(x => x.IsDeleted))).ToEnumerable();
        }

        public bool Insert<T>(T data) where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            collection.InsertOne(data);
            return true;
        }

        public bool Update<T>(T data) where T : Entity
        {
            var collection = MongoFactory.GetOrCreateCollection<T>();
            var result = collection.ReplaceOne(Builders<T>.Filter.Eq(x => x.Id, data.Id), data);
            return result.ModifiedCount > 0;
        }
    }
}
