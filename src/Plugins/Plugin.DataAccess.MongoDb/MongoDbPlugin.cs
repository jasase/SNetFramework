using Framework.Abstraction.Extension;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Services;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Services.DataAccess;
using Plugin.DataAccess.MongoDb.Interfaces;
using Framework.Contracts.Services.DataAccess;

namespace Plugin.DataAccess.MongoDb
{
    public class MongoDbPlugin : Framework.Abstraction.Plugins.Plugin, IGeneralPlugin
    {
        private readonly PluginDescription _description;

        public MongoDbPlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
            : base(resolver, configurator, eventService, logger)
        {
            _description = new PluginDescription
            {
                Name = "MongoDbPlugin",
                ProvidedServices = new[] { typeof(IMongoFactory) },
                NeededServices = new[] { typeof(IConfiguration) }
            };
        }

        public override PluginDescription Description => _description;

        protected override void ActivateInternal()
        {
            var loggerFactory = Resolver.GetInstance<ILogManager>();
            var factory = new MongoFactory(Resolver.GetInstance<MongoSettings>(), loggerFactory.GetLogger(typeof(MongoFactory)));
            factory.LoadMappings();
            var dataAccessProvider = new MongoDataAccessProvider(factory);

            var factoryRegistration = new SingletonRegistration<IMongoFactory>(factory);
            var mongoDataAccessProviderRegistration = new SingletonRegistration<IMongoDataAccessProvider>(dataAccessProvider);
            var dataAccessProviderRegistration = new SingletonRegistration<IDataAccessProvider>(dataAccessProvider);

            ConfigurationResolver.AddRegistration(factoryRegistration);
            ConfigurationResolver.AddRegistration(mongoDataAccessProviderRegistration);
            ConfigurationResolver.AddRegistration(dataAccessProviderRegistration);
        }
    }
}
