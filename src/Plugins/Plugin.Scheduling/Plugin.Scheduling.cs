using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Messages;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Services.Scheduling;

namespace QuartzPlugin
{
    public class QuartzPlugin : Plugin, IGeneralPlugin
    {
        private readonly PluginDescription _description;
        private SchedulingService _service;

        public QuartzPlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
            : base(resolver, configurator, eventService, logger)
        {
            _description = new PluginDescription
            {
                Name = "QuartzPlugin",
                Description = "Liefert Funktionalität um regelmäßig registrierte Task auszuführen",
                ProvidedServices = new[] { typeof(ISchedulingService) }
            };
        }

        public override PluginDescription Description => _description;

        protected override void ActivateInternal()
        {
            _service = new SchedulingService();

            var schedulingServiceRegistration = new SingletonRegistration<ISchedulingService>(_service);
            ConfigurationResolver.AddRegistration(schedulingServiceRegistration);

            _service.StartScheduler();

            EventService.Register<SystemIsShutingDownMessage>(message =>
            {
                _service.StopScheduler();
            });
        }
    }
}
