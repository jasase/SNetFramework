using Framework.Abstraction.Services.Scheduling;
using System;

namespace Framework.Core.Scheduling
{
    public class PollingPlan : IPollingPlan
    {
        private TimeSpan _interval;
        public PollingPlan(TimeSpan interval)
        {
            _interval = interval;
        }

        public TimeSpan Interval
        {
            get { return _interval; }
        }

        public void Accept(ISchedulingPlanVisitor visitor)
        {
            visitor.Handle(this);
        }
    }
}
