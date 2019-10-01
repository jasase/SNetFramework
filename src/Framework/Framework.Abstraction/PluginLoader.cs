using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstraction.Plugins;
using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;

namespace Framework.Abstraction
{
    public class PluginLoader
    {
        private readonly IDependencyResolver _resolver;
        private Dictionary<PluginDescription, IPlugin> _plugins;
        private ILookup<Type, IPlugin> _providedServices;
        private readonly IEnumerable<Type> _pluginTypes;
        private readonly ILogger _logger;

        public PluginLoader(IDependencyResolver resolver, Type[] pluginTypes, ILogger logger)
        {
            _resolver = resolver;
            _pluginTypes = pluginTypes.Concat(new[] { typeof(IGeneralPlugin) });
            _plugins = new Dictionary<PluginDescription, IPlugin>();
            _logger = logger;
        }

        public PluginLoader(IDependencyResolver container, ILogger logger)
            : this(container, new Type[0], logger)
        { }

        public IEnumerable<PluginDescription> Plugins
            => _plugins.Select(x => x.Key);

        public void LoadPlugins()
        {
            _logger.Debug("Loading plugins");
            var instances = QueryContainer().Where(x => x.Description != null).ToArray();
            _logger.Info("Found following plugins:{0}", string.Join(Environment.NewLine, instances.Select(x => x.Description.Name)));
            _plugins = instances.ToDictionary(x => x.Description);
            CreateDependencyTree(instances);
        }

        private IEnumerable<IPlugin> QueryContainer()
        {
            foreach (var type in _pluginTypes)
            {
                if (typeof(IPlugin).IsAssignableFrom(type))
                {
                    foreach (var plugin in _resolver.GetAllInstances(type))
                    {
                        yield return (IPlugin)plugin;
                    }
                }
            }
        }

        private void CreateDependencyTree(IEnumerable<IPlugin> plugins)
        {
            var lookup = (from p in plugins
                          from t in p.Description.ProvidedServices
                          select new { Type = t, Plugin = p }).ToLookup(x => x.Type, x => x.Plugin);
            _providedServices = lookup;
            //TODO Check For cyclic dependencies
        }

        public void ActivatePlugins(PluginDescription plugins)
            => ActivatePlugins(_plugins[plugins]);

        public void ActivatePlugins(IPlugin plugin)
        {
            if (!plugin.IsActivated)
            {
                _logger.Debug("Activating plugin {0}", plugin.GetType().Name);
                foreach (var services in plugin.Description.NeededServices)
                {
                    _logger.Debug("Plugin for dependency {1} of plugin {0} will be searched", plugin.GetType().Name, services.Name);
                    var dependencies = _providedServices[services];
                    var dependedPlugins = dependencies as IPlugin[] ?? dependencies.ToArray();
                    if (!dependedPlugins.Any())
                    {
                        //TODO Eigene Exception
                        throw new Exception(string.Format("Für das Plugin {0} können folgende Abhängigkeiten nicht aufgelöst werden: {1}",
                            plugin.Description.Name,
                            string.Join(", ", services.Name)));
                    }
                    foreach (var dependedPlugin in dependedPlugins)
                    {
                        ActivatePlugins(dependedPlugin);
                    }
                }
                plugin.Activate();
            }
            else
            {
                _logger.Debug("Skipping activation of plugin {0} because it is already active", plugin.GetType().Name);
            }
        }
    }
}
