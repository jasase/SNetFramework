using System;

namespace Framework.Contracts.Services.Scheduling
{
    public interface IPollingPlan : ISchedulingPlan
    {
        TimeSpan Interval { get; }
    }
}
