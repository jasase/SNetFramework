using Framework.Contracts.Extension;
using Framework.Contracts.IocContainer;
using Framework.Contracts.Plugins;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX.Common
{
    public class GpxPlugin : Plugin, IGeneralPlugin
    {
        PluginDescription _description;

        public override PluginDescription Description
        {
            get { return _description; }
        }

        public GpxPlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
            : base(resolver, configurator, eventService, logger)
        {
            _description = new PluginDescription()
            {
                Name = "GPX-Plugin",
                ProvidedServices = new[] { typeof(IGpxDataAccess) }
            };
        }

        protected override void ActivateInternal()
        {
            var registration = new ServiceRegistration<IGpxDataAccess, GpxDataAccess>();
            ConfigurationResolver.AddRegistration(registration);
        }
    }
}
