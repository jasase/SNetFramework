﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Framework.Core.IocContainer;
using Framework.Abstraction;
using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using StructureMap;
using System.Linq;

namespace Framework.Core
{
    public class Bootstrap
    {
        private readonly Action<string> _messageListener;
        private readonly string _assemblyPath;
        private readonly Type[] _pluginTypes;
        private readonly string _folderExtensions;
        private readonly string _folderPlugins;
        protected readonly ILogger Logger;

        private PluginLoader _loader;


        public IDependencyResolver DependencyResolver { get; private set; }
        public IDependencyResolverConfigurator DependencyResolverConfigurator { get; private set; }
        public IContainer StructureMapContainer { get; private set; }
        public IEnumerable<PluginDescription> Plugins => _loader.Plugins;


        public Bootstrap(BootstrapInCodeConfiguration configuration)
        {
            Thread.CurrentThread.Name = "Main";

            _messageListener = configuration.MessageListner.FirstOrDefault(); ;
            _pluginTypes = configuration.PluginTypes.ToArray(); ;
            _folderExtensions = configuration.FolderExtensionPath;
            _folderPlugins = configuration.PluginPath;
            _assemblyPath = Path.GetDirectoryName(GetType().Assembly.Location);

            IntializeIocContainer();
            LoadFrameworkExtensions(configuration);

            Logger = DependencyResolver.GetInstance<ILogManager>().GetLogger(GetType());
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Logger.Error(e.ExceptionObject as Exception, "Unhandled exception occured. Terminating '{0}'", e.IsTerminating);
            };

            var parameter = DependencyResolver.GetInstance<IEnvironmentParameters>();
            LogEnvironmentParameter(parameter);
        }

        protected virtual void BeforeInit()
        { }

        protected virtual void AfterInit()
        { }

        public void Init()
        {
            BeforeInit();

            LoadPlugins();
            ConfigureLoader();
            AfterInit();
        }

        private void LogEnvironmentParameter(IEnvironmentParameters parameter)
        {
            const string format = "{0,-50} | {1}";

            var builder = new StringBuilder();

            builder.AppendLine("Current environment parameters:");

            builder.AppendFormat(format, "Name", "Value");
            builder.AppendLine();

            foreach (var property in parameter.GetType().GetProperties())
            {
                var name = property.Name;
                var value = property.GetMethod.Invoke(parameter, new object[0]);

                builder.AppendFormat(format, name, value);
                builder.AppendLine();
            }

            Logger.Info(builder.ToString());
        }

        private void IntializeIocContainer()
        {
            StructureMapContainer = new Container();
            var dependencyResolver = new StructureMapDependencyResolver(StructureMapContainer);
            Abstraction.IocContainer.DependencyResolver.Setup(dependencyResolver);
            DependencyResolver = dependencyResolver;
            DependencyResolverConfigurator = dependencyResolver;

            StructureMapContainer.Configure(x =>
            {
                x.For<IDependencyResolver>().Singleton().Use(DependencyResolver);
                x.For<IDependencyResolverConfigurator>().Singleton().Use(DependencyResolverConfigurator);
            });
        }

        private void LoadFrameworkExtensions(BootstrapInCodeConfiguration configuration)
        {
            HandleMessage("Scanne nach Extensions");
            var extensionDir = Path.Combine(_assemblyPath, _folderExtensions);

            if (Directory.Exists(extensionDir))
            {
                StructureMapContainer.Configure(cfg => cfg.Scan(scanner =>
                    {
                        scanner.AssembliesFromPath(extensionDir);
                        foreach (var subDir in Directory.GetDirectories(extensionDir))
                        {
                            scanner.AssembliesFromPath(subDir);
                        }

                        scanner.AddAllTypesOf<IFrameworkExtension>();
                    }));

                foreach (var instance in DependencyResolver.GetAllInstances<IFrameworkExtension>())
                {
                    HandleMessage("Lade Extension " + instance.Name);
                    instance.Register(configuration, DependencyResolverConfigurator, DependencyResolver);
                }
            }
        }

        private void LoadPlugins()
        {
            HandleMessage("Scanne nach Plugins");

            var pluginDir = Path.Combine(_assemblyPath, _folderPlugins);

            Logger.Debug("Scanning Assemblies from path {pluginDir}", pluginDir);
            if (Directory.Exists(pluginDir))
            {
                StructureMapContainer.Configure(cfg => cfg.Scan(scanner =>
                    {
                        scanner.TheCallingAssembly();

                        foreach (var loadedAssemblie in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (!loadedAssemblie.IsDynamic && loadedAssemblie.Location.Contains(_assemblyPath))
                            {
                                Logger.Debug("Scanning Assemblies {assembly}", loadedAssemblie.Location);
                                scanner.Assembly(loadedAssemblie);
                            }
                        }

                        foreach (var subDir in Directory.GetDirectories(pluginDir)
                                                        .Concat(new string[] { pluginDir }))
                        {
                            scanner.AssembliesAndExecutablesFromPath(subDir, (string x) =>
                            {
                                Logger.Debug("Scanning Assemblies {assembly}", x);
                                return true;
                            });
                        }

                        scanner.AddAllTypesOf<IGeneralPlugin>();
                        scanner.AddAllTypesOf<ISetting>();

                        foreach (var type in _pluginTypes)
                        {
                            scanner.AddAllTypesOf(type);
                        }
                    }));
            }
        }

        private void ConfigureLoader()
        {
            HandleMessage("Lade Pluginbeschreibungen");
            _loader = StructureMapContainer.With(_pluginTypes).GetInstance<PluginLoader>();
            _loader.LoadPlugins();
        }

        public void ActivatePlugins(PluginDescription[] plugins)
        {
            foreach (var plugin in plugins)
            {
                ActivatePlugins(plugin);
            }
        }

        public void ActivatePlugins(PluginDescription plugin)
        {
            HandleMessage(string.Format("Initialisiere ausgewähltes Plugin {0}", plugin.Name));
            Logger.Debug("Beginne Plugin {0} mit Abhänhgigkeiten zu aktivieren", plugin.Name);
            _loader.ActivatePlugins(plugin);
            Logger.Debug("Plugin {0} mit Abhänhgigkeiten komplett aktiviert", plugin.Name);
        }

        protected void HandleMessage(string message)
        {
            Logger?.Debug("Status message: {msg}", message);
            _messageListener?.Invoke(message);
        }
    }
}
