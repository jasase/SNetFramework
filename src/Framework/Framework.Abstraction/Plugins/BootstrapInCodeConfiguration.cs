using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Abstraction.Plugins
{
    public class BootstrapInCodeConfiguration
    {
        public List<Action<string>> MessageListner { get; }
        public List<Type> PluginTypes { get; }
        private Dictionary<Type, BootstrapExtensionConfiguration> ExtensionConfiguration { get; }
        public string FolderExtensionPath { get; private set; }
        public string PluginPath { get; private set; }

        private BootstrapInCodeConfiguration()
        {
            MessageListner = new List<Action<string>>();
            PluginTypes = new List<Type>();
            ExtensionConfiguration = new Dictionary<Type, BootstrapExtensionConfiguration>();
            FolderExtensionPath = ".";
            PluginPath = ".";
        }

        public static BootstrapInCodeConfiguration Default()
            => new BootstrapInCodeConfiguration();

        public BootstrapInCodeConfiguration AddMessageListener(Action<string> messageListner)
        {
            MessageListner.Add(messageListner);
            return this;
        }

        public BootstrapInCodeConfiguration AddPluginType<TPlugin>()
            where TPlugin : class
        {
            PluginTypes.Add(typeof(TPlugin));
            return this;
        }

        public BootstrapInCodeConfiguration SetFolderExtensionPath(string path)
        {
            FolderExtensionPath = path;
            return this;
        }

        public BootstrapInCodeConfiguration SetPluginPath(string path)
        {
            PluginPath = path;
            return this;
        }

        public BootstrapInCodeConfiguration AddExtensionConfiguration<TConfiguration>(Action<TConfiguration> configurationBuilder)
        {
            ExtensionConfiguration[typeof(TConfiguration)] = new BootstrapExtensionConfiguration<TConfiguration>(configurationBuilder);
            return this;
        }

        public void ConfigureExtensionConfiguration<TConfiguration>(TConfiguration configuration)
        {
            var type = typeof(TConfiguration);
            if (ExtensionConfiguration.ContainsKey(type))
            {
                var builder = ExtensionConfiguration[type] as BootstrapExtensionConfiguration<TConfiguration>;
                builder.ConfigurationBuilder(configuration);
            }
        }

        private abstract class BootstrapExtensionConfiguration
        {
            public BootstrapExtensionConfiguration(Type configurationType)
            {
                ConfigurationType = configurationType;
            }

            public Type ConfigurationType { get; }
        }

        private class BootstrapExtensionConfiguration<TConfiguration> : BootstrapExtensionConfiguration
        {
            public BootstrapExtensionConfiguration(Action<TConfiguration> configurationBuilder)
                : base(typeof(TConfiguration))
            {
                ConfigurationBuilder = configurationBuilder;
            }

            public Action<TConfiguration> ConfigurationBuilder { get; }
        }

    }
}

