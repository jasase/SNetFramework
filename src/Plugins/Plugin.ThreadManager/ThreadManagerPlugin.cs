using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Services.ThreadManaging;
using System;

namespace Plugin.ThreadManagers
{
    public class ThreadManagerPlugin : Framework.Abstraction.Plugins.Plugin, IGeneralPlugin
    {
        private readonly PluginDescription _description;
        public override PluginDescription Description => _description;

        public ThreadManagerPlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger) : base(resolver, configurator, eventService, logger)
        {
            _description = new PluginDescription
            {
                Name = "ThreadManager",
                Description = "Plugin zum verwalten von Threads",
                ProvidedServices = new[] { typeof(IThreadManager) }
            };
        }

        protected override void ActivateInternal()
            => ConfigurationResolver.AddRegistration(new SingletonRegistration<IThreadManager, ThreadManager>());
    }
}
