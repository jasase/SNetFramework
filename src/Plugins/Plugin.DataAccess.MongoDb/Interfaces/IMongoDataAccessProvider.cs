using Framework.Abstraction.Services.DataAccess;
using Plugin.DataAccess.MongoDb.Interfaces;

namespace Framework.Contracts.Services.DataAccess
{
    public interface IMongoDataAccessProvider : IDataAccessProvider
    {
        IMongoFactory MongoFactory { get; }
    }
}
