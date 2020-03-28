using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;

namespace Extension.EnvironmentParameter
{
    public class EnvironmentParameterExtension : IFrameworkExtension
    {        
        public string Name => "Extension.EnvironmentParameter";

        public void Register(BootstrapInCodeConfiguration configuration, IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            var parameter = new EnvironmentParameter();
            var registration = new SingletonRegistration<IEnvironmentParameters>(parameter);

            configurator.AddRegistration(registration);            
        }
    }
}
