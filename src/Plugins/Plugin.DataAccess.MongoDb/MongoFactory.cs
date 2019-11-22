using System;
using System.Linq;
using System.Text;
using Framework.Abstraction.Extension;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Plugin.DataAccess.MongoDb.Interfaces;


namespace Plugin.DataAccess.MongoDb
{
    public class MongoFactory : IMongoFactory
    {
        private readonly object _gridFsCreateLock;
        private readonly MongoSettings _settings;
        private readonly ILogger _logger;
        private readonly IMongoDatabase _database;

        private GridFSBucket<Guid> _gridFs;

        public MongoFactory(MongoSettings settings, ILogger logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _gridFsCreateLock = new object();
            var mongoClientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
            //mongoClientSettings.ClusterConfigurator = cb =>
            //{
            //    cb.Subscribe<CommandStartedEvent>(e =>
            //    {
            //        Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
            //    });
            //};
            var client = new MongoClient(mongoClientSettings);

            client.ListDatabases();

            _database = client.GetDatabase(_settings.DbName);
        }

        internal void LoadMappings()
        {
            var mapTypes = from a in AppDomain.CurrentDomain.GetAssemblies()
                           from t in a.GetTypes()
                           where t.BaseType != null
                           where !t.IsAbstract
                           where t.BaseType.IsGenericType
                           where t.BaseType.GetGenericTypeDefinition() == typeof(BsonClassMap<>)
                           select t;

            _logger.Debug("Loaded mappings:\r\n{0}", () =>
            {
                var columnLength = 161;
                var format = "{0,-80} {1,80}" + Environment.NewLine;
                var sb = new StringBuilder();
                sb.AppendFormat(format, "Entity", "Mapping");
                sb.Append('-', columnLength);
                sb.AppendLine();

                foreach (var mapping in mapTypes.Select(x => new { Mapping = x.FullName, Entity = x.BaseType.GenericTypeArguments[0].FullName })
                                                .OrderBy(x => x.Entity))
                {
                    sb.AppendFormat(format, mapping.Entity, mapping.Mapping);
                }

                sb.Append('-', columnLength);
                sb.AppendLine();

                return sb.ToString();
            });

            foreach (var map in mapTypes)
            {
                var mapClass = (BsonClassMap) Activator.CreateInstance(map);
                BsonClassMap.RegisterClassMap(mapClass);
            }
        }

        public IMongoCollection<T> GetOrCreateCollection<T>()
            => _database.GetCollection<T>(GetCollectionName<T>());


        public IMongoCollection<BsonDocument> GetOrCreateCollectionBson<T>()
            => _database.GetCollection<BsonDocument>(GetCollectionName<T>());

        public GridFSBucket<Guid> GetGridFsBucket()
        {
            if (_gridFs != null)
            {
                return _gridFs;
            }

            lock (_gridFsCreateLock)
            {
                if (_gridFs == null)
                {
                    _gridFs = new GridFSBucket<Guid>(_database, new GridFSBucketOptions
                    {
                        ChunkSizeBytes = 255 * 1024
                    });
                }
            }

            return _gridFs;
        }

        private string GetCollectionName<T>()
            => typeof(T).Name;
    }
}
