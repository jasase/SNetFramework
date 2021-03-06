﻿using System;
using Framework.Abstraction.Services.Scheduling;

namespace Framework.Core.Scheduling
{
    public class DailyPlan : IDailyPlan

    {
        public TimeSpan TimeOfDay { get; private set; }

        public DailyPlan(TimeSpan timeOfDay)
        {
            if (timeOfDay == null ||
               timeOfDay.Days > 0)
            {
                throw new ArgumentException(nameof(timeOfDay));
            }

            TimeOfDay = timeOfDay;
        }

        public void Accept(ISchedulingPlanVisitor visitor)
            => visitor.Handle(this);
    }
}
