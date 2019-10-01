using Framework.Contracts.Extension;
using Framework.Contracts.IocContainer;
using Framework.Contracts.Plugins;
using NLog.Targets;

namespace Extension.LoggingNLog
{
    public class LoggingExtension : IFrameworkExtension
    {
        public string Name => "Logging-NLog";

        public void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            //Target.Register<Fluentd.Fluentd>("Fluentd");

            configurator.AddRegistration(new SingletonRegistration<ILogManager>(new LogManager()));
            configurator.AddRegistration(new FactoryRegistration<ILogger, ILogManager>(
                (factory, context) => factory.GetLogger(context.ParentType)));
        }
    }
}
