using System;
using System.Collections.Generic;
using System.Text;
using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using ServiceHost.Contracts;

namespace Tests.Services.Start
{
    public class DummyPlugin : Plugin, IServicePlugin
    {
        public static int StartCounter { get; set; } = 0;

        public DummyPlugin(IDependencyResolver resolver,
                           IDependencyResolverConfigurator configurator,
                           IEventService eventService,
                           ILogger logger)
            : base(resolver, configurator, eventService, logger)
        { }

        public override PluginDescription Description => new AutostartServicePluginDescription
        {
            Name = "Test"
        };

        protected override void ActivateInternal()
            => StartCounter += 1;
    }
}
