using System;
using System.IO;
using System.Reflection;
using Framework.Abstraction;
using Framework.Abstraction.Extension;

namespace Extension.EnvironmentParameter
{
    public class EnvironmentParameter : IEnvironmentParameters
    {
        public DirectoryInfo ConfigurationDirectory { get; private set; }

        public DirectoryInfo ApplicationDirectory { get; private set; }

        public FileInfo ExecutablePath { get; private set; }

        public EnvironmentParameter()
        {
            ConfigurationDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "MiniFramework", "Configuration"));
            ApplicationDirectory = new DirectoryInfo(Path.GetDirectoryName(typeof(PluginLoader).Assembly.Location));
            ExecutablePath = new FileInfo(Assembly.GetEntryAssembly().Location);

            if (!ConfigurationDirectory.Exists)
            {
                ConfigurationDirectory.Create();
            }
        }
    }
}
