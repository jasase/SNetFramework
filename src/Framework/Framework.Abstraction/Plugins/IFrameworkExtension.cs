using Framework.Contracts.IocContainer;

namespace Framework.Contracts.Plugins
{
    public interface IFrameworkExtension
    {
        string Name { get; }
        void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver);
    }
}
