using System;

namespace Framework.Contracts.Services.Scheduling
{
    public interface IOneTimePlan : ISchedulingPlan
    {
        TimeSpan StartDelay { get; }
    }
}
