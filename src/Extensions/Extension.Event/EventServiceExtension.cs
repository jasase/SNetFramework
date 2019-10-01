using EventExtension;
using Framework.Contracts.Extension;
using Framework.Contracts.IocContainer;
using Framework.Contracts.Plugins;

namespace Extension.Event
{
    public class EventServiceExtension : IFrameworkExtension
    {
        public string Name => "EventService";

        public void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            configurator.AddRegistration(new SingletonRegistration<IEventService, EventService>());
        }
    }
}
