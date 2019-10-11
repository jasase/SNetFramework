using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Messages;
using ServiceHost.Contracts;

namespace ServiceHost.Docker
{
    public class DockerBootstrap : Bootstrap
    {

        public DockerBootstrap()
            : base(null, new[] { typeof(IServicePlugin) }, ".", ".")
        { }

        public void StartingService(IEnumerable<string> pluginsToLoad)
        {
            try
            {                
                Init();

                var pluginsToActivate = Plugins.Where(x => x.GetType() == typeof(AutostartServicePluginDescription)).ToArray();

                if (pluginsToActivate.Any())
                {
                    ActivatePlugins(pluginsToActivate);
                }
                else
                {
                    Logger.Error("No plugin selected. Available plugins are; {0}",
                                 string.Join(", ", Plugins.Select(x => x.Name.ToUpperInvariant())));
                    Environment.Exit(-10);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occured during starting the service");
                Environment.Exit(-1);
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
                Logger.Error(ex, "An error occured during service shutdown");
            }
        }
    }
}
