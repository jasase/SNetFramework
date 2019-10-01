using Framework.Abstraction.IocContainer;

namespace Framework.Abstraction.Plugins
{
    public interface IFrameworkExtension
    {
        string Name { get; }
        void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver);
    }
}
