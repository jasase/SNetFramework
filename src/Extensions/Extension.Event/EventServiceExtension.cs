using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;

namespace Extension.Event
{
    public class EventServiceExtension : IFrameworkExtension
    {
        public string Name => "Extension.Event";

        public void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
            => configurator.AddRegistration(new SingletonRegistration<IEventService, EventService>());
    }
}
