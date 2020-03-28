using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Framework.Abstraction.Extension
{
    public interface IMetricRecorder
    {
        void CountEvent([NotNull] MetricIdentifier identifier);
        void CountEvent([NotNull] MetricIdentifier identifier, string itemType);

        void Measure([NotNull] MetricIdentifier identifier, double value);
        void Measure([NotNull] MetricIdentifier identifier, double value, string itemType);
    }
}
