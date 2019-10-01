using System.IO;
using System.Reflection;
using Framework.Abstraction.Extension;

namespace Extension.EnvironmentParameterDocker
{
    public class EnvironmentParameter : IEnvironmentParameters
    {
        public DirectoryInfo ConfigurationDirectory { get; private set; }

        public DirectoryInfo ApplicationDirectory { get; private set; }

        public FileInfo ExecutablePath { get; private set; }

        public EnvironmentParameter()
        {
            ConfigurationDirectory = new DirectoryInfo(@"C:\Configuration");
            ApplicationDirectory = new DirectoryInfo(@"C:\App");
            ExecutablePath = new FileInfo(Assembly.GetExecutingAssembly().Location);

            if (!ConfigurationDirectory.Exists)
            {
                ConfigurationDirectory.Create();
                ConfigurationDirectory.Refresh();
            }
        }
    }
}
