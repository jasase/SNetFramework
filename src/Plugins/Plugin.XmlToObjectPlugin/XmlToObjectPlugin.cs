using Framework.Contracts.Extension;
using Framework.Contracts.Extension.EventService;
using Framework.Contracts.IocContainer;
using Framework.Contracts.Plugins;
using Framework.Contracts.Services.XmlToObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToObjectPlugin
{
    public class XmlToObjectPlugin : Plugin, IGeneralPlugin
    {
        private PluginDescription _description;

        public XmlToObjectPlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
            : base(resolver, configurator, eventService, logger)
        {
            _description = new PluginDescription
            {
                Name = "XML To Object Converter",
                Description = "Liest XML Dateien aus und befüllt die dazugehörigen Objekte",
                ProvidedServices = new[] { typeof(IXmlToObject) }
            };
        }

        public override PluginDescription Description => _description;

        protected override void ActivateInternal()
        {
            var registraton = new ServiceRegistration<IXmlToObject, XmlToObject>();
            ConfigurationResolver.AddRegistration(registraton);
        }
    }
}
