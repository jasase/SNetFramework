using System;

namespace Framework.Abstraction.Services.Scheduling
{
    public interface IPollingPlan : ISchedulingPlan
    {
        TimeSpan Interval { get; }
    }
}
