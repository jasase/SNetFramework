using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Abstraction.Extension
{
    public interface IMetricRecorder
    {
        void CountEvent(MetricIdentifier identifier);
        void CountEvent(MetricIdentifier identifier, string itemType);

        void Measure(MetricIdentifier identifier, double value);
        void Measure(MetricIdentifier identifier, double value, string itemType);
    }
}
