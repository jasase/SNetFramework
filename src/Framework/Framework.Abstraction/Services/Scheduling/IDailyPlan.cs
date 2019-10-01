using System;

namespace Framework.Abstraction.Services.Scheduling
{
    public interface IDailyPlan : ISchedulingPlan
    {
        TimeSpan TimeOfDay { get; }
    }
}
