using System;
using Service;
using Topshelf;

namespace ServiceHost
{

    public class Program
    {
        public const string SERVICE_NAME = "FrameworkService";

        static void Main(string[] args)
            => HostFactory.Run(x =>
            {
                x.SetDisplayName(SERVICE_NAME);
                x.SetServiceName(SERVICE_NAME);
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
