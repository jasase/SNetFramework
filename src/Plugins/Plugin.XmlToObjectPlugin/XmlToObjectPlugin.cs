﻿using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Services.XmlToObject;

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
