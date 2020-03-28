using System;
using System.Collections.Generic;
using System.Text;
using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Plugins;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Extension.TelemetryApplicationInsights
{
    public class TelemetryExtension : IFrameworkExtension
    {
        public string Name => "Application-Insights";

        public void Register(BootstrapInCodeConfiguration configuration, IDependencyResolverConfigurator configurator, IDependencyResolver resolver)
        {
            var appInsightConfig = TelemetryConfiguration.CreateDefault();
            configuration.ConfigureExtensionConfiguration(appInsightConfig);


            var telemetryClient = new TelemetryClient(appInsightConfig);
            telemetryClient.TrackTrace("Telemetry started");

            var metricRecorder = new TelemetryRecorder(telemetryClient);

            configurator.AddRegistration(new SingletonRegistration<IMetricRecorder>(metricRecorder));
            configurator.AddRegistration(new SingletonRegistration<TelemetryClient>(telemetryClient));
        }
    }
}
