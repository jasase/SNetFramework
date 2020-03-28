
using System;
using System.Collections.Generic;
using System.Text;
using Framework.Abstraction.Extension;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Extension.TelemetryApplicationInsights
{
    public class TelemetryRecorder : IMetricRecorder
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly Dictionary<MetricIdentifier, Metric> _metrics;


        public TelemetryRecorder(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
            _metrics = new Dictionary<MetricIdentifier, Metric>();
        }

        public void CountEvent(MetricIdentifier identifier)
        {
            var name = identifier.ToString();
            _telemetryClient.TrackEvent(name);
        }

        public void CountEvent(MetricIdentifier identifier, string itemType)
        {
            throw new NotImplementedException();
        }

        public void Measure(MetricIdentifier identifier, double value)
        {
            var metric = GetMetric(identifier);
            metric.TrackValue(value);
        }

        public void Measure(MetricIdentifier identifier, double value, string itemType)
        {
            throw new NotImplementedException();
        }

        private Metric GetMetric(MetricIdentifier identifier)
        {
            lock (_metrics)
            {
                if (!_metrics.ContainsKey(identifier))
                {
                    var metric = _telemetryClient.GetMetric(identifier.ToString());
                    _metrics[identifier] = metric;
                }

                return _metrics[identifier];
            }
        }
    }
}
