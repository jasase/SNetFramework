using Framework.Abstraction.Services.Scheduling;
using Quartz;
using System;

namespace QuartzPlugin
{
    public class SchedulingTriggerVisitor : ISchedulingPlanVisitor
    {
        private IJobDetail _job;

        public SchedulingTriggerVisitor(IJobDetail job)
        {
            _job = job ?? throw new ArgumentNullException("job");
        }

        public ITrigger Trigger { get; private set; }

        public void Handle(IWeekleyPlan plan)
            => throw new NotImplementedException();

        public void Handle(IPollingPlan plan)
            => Trigger = TriggerBuilder.Create()
                                        .ForJob(_job)
                                        .StartNow()
                                        .WithSimpleSchedule(x => x.WithInterval(plan.Interval).RepeatForever())
                                        .Build();

        public void Handle(IOneTimePlan plan)
            => Trigger = TriggerBuilder.Create()
                                        .ForJob(_job)
                                        .StartAt(DateTime.Now.Add(plan.StartDelay))
                                        .Build();

        public void Handle(IDailyPlan plan)
        {
            var now = DateTime.Now;
            if (now.TimeOfDay > plan.TimeOfDay)
            {
                now = now.AddDays(1);
            }
            now.Add(plan.TimeOfDay - now.TimeOfDay);
            var tof = new TimeOfDay(plan.TimeOfDay.Hours,
                                    plan.TimeOfDay.Minutes,
                                    plan.TimeOfDay.Seconds);

            Trigger = TriggerBuilder.Create()
                                        .ForJob(_job)
                                        .StartAt(now)
                                        .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(tof))
                                        .Build();
        }
    }
}
