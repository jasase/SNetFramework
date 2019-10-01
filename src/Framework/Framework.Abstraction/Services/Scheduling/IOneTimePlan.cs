using System;

namespace Framework.Abstraction.Services.Scheduling
{
    public interface IOneTimePlan : ISchedulingPlan
    {
        TimeSpan StartDelay { get; }
    }
}
