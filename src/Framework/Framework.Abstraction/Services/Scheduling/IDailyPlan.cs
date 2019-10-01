using System;

namespace Framework.Contracts.Services.Scheduling
{
    public interface IDailyPlan : ISchedulingPlan
    {
        TimeSpan TimeOfDay { get; }
    }
}
