using System;

namespace Framework.Contracts.Services.Scheduling
{
    public interface IWeekleyPlan : ISchedulingPlan
    {
        DayOfWeek WeekOfDay { get; }

        int Hour { get; }
        int Minute { get; }
        int Second { get; }
    }
}
