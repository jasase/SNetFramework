using System.Collections.Generic;
using System.Linq;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Services;
using Framework.Abstraction.Services.XmlToObject;
using ConfigurationPlugin.Configuration;
using System.Text;
using System;
using Framework.Abstraction.IocContainer;
using Plugin.Configuration.Loaders;

namespace ConfigurationPlugin
{
    public class ConfigurationPlugin : Framework.Abstraction.Plugins.Plugin, IGeneralPlugin
    {

        private readonly PluginDescription _description;

        public ConfigurationPlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
            : base(resolver, configurator, eventService, logger)
        {
            _description = new PluginDescription
            {
                Name = "Konfiguration",
                NeededServices = new[] { typeof(IXmlToObject) },
                ProvidedServices = new[] { typeof(IConfiguration) }
            };
        }

        public override PluginDescription Description
            => _description;

        protected override void ActivateInternal()
        {
            var loader = Resolver.CreateConcreteInstanceWithDependencies<ConfigurationLoader>();
            var envLloader = Resolver.CreateConcreteInstanceWithDependencies<EnvConfigurationLoader>();
            var settingValues = loader.LoadConfiguration()
                                      .Concat(envLloader.LoadConfiguration()).ToList();

            LogLoadedConfiguration(settingValues);

            var settingsInitializer = Resolver.CreateConcreteInstanceWithDependencies<ConfigurationObjectInitializer>();
            settingsInitializer.Initialice(settingValues);
        }

        private void LogLoadedConfiguration(IEnumerable<Area> settingValues)
        {
            var builder = new StringBuilder();

            foreach (var area in settingValues)
            {
                builder.AppendFormat("Area: {0}", area.Name);
                builder.AppendLine();
                LogAreaElements(area.Elements, builder, 1);
            }

            Logger.Info(builder.ToString());
        }

        private void LogAreaElements(IList<AreaElement> elements, StringBuilder builder, int deep)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                var visitor = new LogElementVisitor(builder, deep, i == elements.Count - 1);
                element.Accept(visitor);
            }
        }

        private class LogElementVisitor : IAreaElementVisitor
        {
            private StringBuilder _builder;
            private int _deep;
            private string _ident;
            private string _treeSign;

            public LogElementVisitor(StringBuilder builder, int deep, bool isLast)
            {
                _builder = builder;
                _deep = deep;
                var chars = Enumerable.Range(0, deep * 5).Select(x => ' ').ToArray();
                _ident = new String(chars);

                _treeSign = isLast ? "└─" : "├─";
            }

            public void Handle(Algorithm setting)
            {
                _builder.AppendFormat("{0}{3} {1}: {2}", _ident, setting.Key, setting.Name, _treeSign);
                _builder.AppendLine();

                var visitor = new LogElementVisitor(_builder, _deep + 1, false);
                for (int i = 0; i < setting.Settings.Count; i++)
                {
                    if (i == setting.Settings.Count - 1)
                    {
                        var visitorLast = new LogElementVisitor(_builder, _deep + 1, true);
                        setting.Settings[i].Accept(visitorLast);
                    }
                    else
                    {
                        setting.Settings[i].Accept(visitor);
                    }
                }
            }

            public void Handle(Setting setting)
            {
                _builder.AppendFormat("{0}{3} {1}: {2}", _ident, setting.Key, setting.Value, _treeSign);
                _builder.AppendLine();
            }
        }
    }
}
