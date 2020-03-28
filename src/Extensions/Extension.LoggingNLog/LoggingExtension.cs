using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using NLog.Targets;

namespace Extension.LoggingNLog
{
    public class LoggingExtension : IFrameworkExtension
    {
        public string Name => "Extension.LoggingNLog";

        public void Register(BootstrapInCodeConfiguration configuration, IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            //Target.Register<Fluentd.Fluentd>("Fluentd");

            configurator.AddRegistration(new SingletonRegistration<ILogManager>(new LogManager()));
            configurator.AddRegistration(new FactoryRegistration<ILogger, ILogManager>(
                (factory, context) => factory.GetLogger(context.ParentType)));
        }
    }
}
