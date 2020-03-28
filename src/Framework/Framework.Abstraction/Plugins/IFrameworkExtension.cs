using Framework.Abstraction.IocContainer;

namespace Framework.Abstraction.Plugins
{
    public interface IFrameworkExtension
    {
        string Name { get; }
        void Register(BootstrapInCodeConfiguration configuration, IDependencyResolverConfigurator configurator, IDependencyResolver resolver);
    }
}
