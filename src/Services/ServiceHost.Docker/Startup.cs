using System;
using System.Collections.Generic;
using Framework.Abstraction.Plugins;
using Topshelf;

namespace ServiceHost.Docker
{
    public abstract class Startup
    {
        protected void Run(string[] args, BootstrapInCodeConfiguration configuration)
        {
            Console.WriteLine("Start");
            var pluginsToLoad = new List<string>();
            HostFactory.Run(x =>
            {
                x.UseNLog();
                x.Service<DockerBootstrap>(svc =>
                {
                    svc.ConstructUsing(() => new DockerBootstrap(configuration));
                    svc.WhenStarted(s => s.StartingService(pluginsToLoad));
                    svc.WhenStopped(s => s.StopingService());
                });

                x.AddCommandLineDefinition("plugin", p => pluginsToLoad.Add(p));
                x.OnException(e =>
                {
                    Console.WriteLine(e.ToString());
                });
            });
            Console.WriteLine("End");
        }
    }
}
