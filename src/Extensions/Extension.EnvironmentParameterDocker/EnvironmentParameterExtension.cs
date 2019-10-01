using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;

namespace Extension.EnvironmentParameterDocker
{
    public class EnvironmentParameterExtension : IFrameworkExtension
    {        
        public string Name => "Extension.EnvironmentParameterDocker";

        public void Register(IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            var parameter = new EnvironmentParameter();
            var registration = new SingletonRegistration<IEnvironmentParameters>(parameter);

            configurator.AddRegistration(registration);            
        }
    }
}
