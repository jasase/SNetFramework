using System;
using System.Diagnostics.CodeAnalysis;
using Framework.Abstraction.Plugins;
using Microsoft.ApplicationInsights.Extensibility;

namespace Extension.TelemetryApplicationInsights
{
    public static class TelemetryExtensionConfiguration
    {
        public static BootstrapInCodeConfiguration ConfigureTelemetry([NotNull] this BootstrapInCodeConfiguration codeConfiguration,
                                                               Action<TelemetryConfiguration> configurationBuilder)
            => codeConfiguration.AddExtensionConfiguration(configurationBuilder);
    }
}
