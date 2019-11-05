using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ConfigurationDirectory = new DirectoryInfo(@"C:\Configuration");
                ApplicationDirectory = new DirectoryInfo(@"C:\App");
            }
            else
            {
                ConfigurationDirectory = new DirectoryInfo(@"/configuration");
                ApplicationDirectory = new DirectoryInfo(@"/app");
            }


            ExecutablePath = new FileInfo(Assembly.GetExecutingAssembly().Location);

            if (!ConfigurationDirectory.Exists)
            {
                ConfigurationDirectory.Create();
                ConfigurationDirectory.Refresh();
            }
        }
    }
}
