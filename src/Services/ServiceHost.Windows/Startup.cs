using System;
using Service;
using Topshelf;

namespace ServiceHost
{

    public abstract class Startup
    {
        private readonly string _serviceName;

        public Startup(string serviceName)
        {
            _serviceName = serviceName;
        }

        protected void Run(string[] args)
            => HostFactory.Run(x =>
            {
                x.SetDisplayName(_serviceName);
                x.SetServiceName(_serviceName);
                x.UseNLog();
                x.Service<ServiceBootstrap>(sc =>
                {
                    sc.ConstructUsing(() => new ServiceBootstrap());
                    sc.WhenStarted(s => s.StartingService());
                    sc.WhenStopped(s =>
                    {
                        try
                        {
                            s.StopingService();
                        }
                        catch (Exception)
                        { }
                    });
                });
            });
    }
}
