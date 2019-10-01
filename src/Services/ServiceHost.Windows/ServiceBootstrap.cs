using System;
using System.Linq;
using Framework.Core;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Messages;
using ServiceHost.Contracts;

namespace Service
{
    public class ServiceBootstrap : Bootstrap
    {
        public ServiceBootstrap()
            : base(null, new[] { typeof(IServicePlugin) })
        { }

        public void StartingService()
        {
            try
            {
                Init();
                var autoStartPlugins = Plugins.Where(x => x.GetType() == typeof(AutostartServicePluginDescription)).ToArray();

                foreach (var autoStartPlugin in autoStartPlugins)
                {
                    Logger.Info("Plugin {0} marked for autostart", autoStartPlugin.Name);
                }

                ActivatePlugins(autoStartPlugins);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occured during starting the service");
                //TODO Shutdown System
            }            
        }

        public void StopingService()
        {            
            try
            {
                var eventService = DependencyResolver.GetInstance<IEventService>();
                eventService.Publish(new SystemIsShutingDownMessage());
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"An error occured during service shutdown");
            }            
        }
    }
}
