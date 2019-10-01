﻿using System;
using System.Collections.Generic;
using Topshelf;

namespace ServiceHost.Docker
{
    public abstract class Program
    {
        protected void Main(string[] args)
        {
            Console.WriteLine("Start");
            var pluginsToLoad = new List<string>();
            HostFactory.Run(x =>
            {
                x.UseNLog();
                x.Service<DockerBootstrap>(svc =>
                {
                    svc.ConstructUsing(() => new DockerBootstrap());
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
