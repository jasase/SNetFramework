using System;
using Framework.Contracts.Services.Scheduling;

namespace Framework.Common.Scheduling
{
    public class OneTimePlan : IOneTimePlan
    {
        public TimeSpan StartDelay { get; private set; }

        public OneTimePlan(TimeSpan startDelay)
        {
            StartDelay = startDelay;
        }

        public void Accept(ISchedulingPlanVisitor visitor)
            => visitor.Handle(this);
    }
}
