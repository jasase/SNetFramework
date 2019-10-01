using System;
using Framework.Contracts.Messages;
using Framework.Contracts.Extension;
using Framework.Contracts.IocContainer;
using System.Collections.Generic;

namespace Framework.Contracts.Plugins
{
    public abstract class Plugin : IPlugin
    {
        private readonly IDependencyResolver _resolver;
        private readonly IDependencyResolverConfigurator _configuratiorResolver;
        private bool _isActive;
        private readonly IEventService _eventService;
        private readonly ILogger _logger;

        public IDependencyResolver Resolver { get { return _resolver; } }
        public IDependencyResolverConfigurator ConfigurationResolver { get { return _configuratiorResolver; } }
        public IEventService EventService { get { return _eventService; } }
        public bool IsActivated { get { return _isActive; } }
        public ILogger Logger { get { return _logger; } }
        public abstract PluginDescription Description { get; }

        protected Plugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (configurator == null) throw new ArgumentNullException("configurator");
            if (eventService == null) throw new ArgumentNullException("eventService");
            if (logger == null) throw new ArgumentNullException("logger");

            _eventService = eventService;
            _resolver = resolver;
            _configuratiorResolver = configurator;
            _isActive = false;
            _logger = logger;
        }

        public void Activate()
        {
            if (_isActive == false)
            {
                Logger.Debug("Activate Plugin {0}", Description.Name);
                _eventService.Publish(new PluginIsLoadingMessage(Description));

                ActivateInternal();

                _isActive = true;
                _eventService.Publish(new PluginIsLoadedMessage(Description));
                Logger.Debug("Plugin {0} succesfully activated", Description.Name);

                return;
            }
            _logger.Error("Das Plugin {0} wurde bereits zuvor schon aktiviert", Description.Name);
            throw new InvalidOperationException("Plugin ist bereits aktiviert worden");
        }

        protected abstract void ActivateInternal();

        protected void ApplyRegistrations(IEnumerable<DependencyResolverRegistration> registrations)
        {
            foreach (var registration in registrations)
            {
                ConfigurationResolver.AddRegistration(registration);
            }
        }
    }
}
