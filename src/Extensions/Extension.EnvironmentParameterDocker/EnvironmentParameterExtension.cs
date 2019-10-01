using Framework.Contracts.Extension;
using Framework.Contracts.IocContainer;
using Framework.Contracts.Plugins;

namespace EnvironmentPlugin
{
    public class EnvironmentParameterExtension : IFrameworkExtension
    {        
        public string Name => "EnvironmentParameterExtensionDocker";

        public void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            var parameter = new EnvironmentParameter();
            var registration = new SingletonRegistration<IEnvironmentParameters>(parameter);

            configurator.AddRegistration(registration);            
        }
    }
}
