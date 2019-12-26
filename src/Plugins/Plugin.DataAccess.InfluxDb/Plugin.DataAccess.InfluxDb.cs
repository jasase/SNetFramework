using AdysTech.InfluxDB.Client.Net;
using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Services;
using Framework.Abstraction.Services.DataAccess.InfluxDb;
using Framework.Abstraction.Services.Scheduling;
using Framework.Abstraction.Services.ThreadManaging;
using Plugin.DataAccess.InfluxDb.Services;
using System;

namespace Plugin.DataAccess.InfluxDb
{
    public class InfluxDbPlugin : Framework.Abstraction.Plugins.Plugin, IGeneralPlugin
    {
        private PluginDescription _pluginDescription;

        public override PluginDescription Description => _pluginDescription;

        public InfluxDbPlugin(IDependencyResolver resolver,
                              IDependencyResolverConfigurator configurator,
                              IEventService eventService,
                              ILogger logger) :
            base(resolver, configurator, eventService, logger)
        {
            _pluginDescription = new PluginDescription
            {
                Description = "Access to time series database influx db",
                Name = "InfluxDbPlugin",
                NeededServices = new Type[] { typeof(IConfiguration), typeof(IThreadManager), typeof(ISchedulingService) },
                ProvidedServices = new Type[] { typeof(IInfluxDbUpload) }
            };
        }

        protected override void ActivateInternal()
        {
            var setting = Resolver.GetInstance<InfluxDbSetting>();
            var threadManager = Resolver.GetInstance<IThreadManager>();
            var schedulingService = Resolver.GetInstance<ISchedulingService>();
            var loggerFactory = Resolver.GetInstance<ILogManager>();

            Logger.Info("Establishing connection to server [{0}]", setting.Server);
            var influxDbClient = new InfluxDBClient(setting.Server, setting.Username, setting.Password);

            var upload = new InfluxDbUpload(loggerFactory.GetLogger(typeof(InfluxDbUpload)),
                                            influxDbClient,
                                            threadManager,
                                            schedulingService);
            var uploadRegistration = new SingletonRegistration<IInfluxDbUpload>(upload);
            ConfigurationResolver.AddRegistration(uploadRegistration);

            var read = new InfluxDbRead(influxDbClient);
            var readRegistration = new SingletonRegistration<IInfluxDbRead>(read);
            ConfigurationResolver.AddRegistration(readRegistration);

            var management = new InfluxDbManagement(influxDbClient, loggerFactory.GetLogger(typeof(InfluxDbManagement)));
            var managementRegistration = new SingletonRegistration<IInfluxDbManagement>(management);
            ConfigurationResolver.AddRegistration(managementRegistration);
        }
    }
}
