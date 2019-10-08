using Framework.Abstraction.Services.Scheduling;
using Quartz;
using Quartz.Impl;
using System;

namespace QuartzPlugin
{
    public class SchedulingService : ISchedulingService
    {
        private IScheduler _scheduler;

        public SchedulingService()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler().Result;
        }

        public void StartScheduler()
            => _scheduler.Start().Wait();

        public void StopScheduler()
            => _scheduler.Shutdown(false).Wait();

        public Tuple<Framework.Abstraction.Services.Scheduling.IJob, ISchedulingPlan> ActiveSchedulings
            => throw new NotImplementedException();

        public void AddJob(Framework.Abstraction.Services.Scheduling.IJob job, ISchedulingPlan plan)
        {
            var quartzJob = CreateJob(job);
            var trigger = CreateTrigger(plan, quartzJob);
            _scheduler.ScheduleJob(quartzJob, trigger);
        }

        public void RemoveJob(Framework.Abstraction.Services.Scheduling.IJob job, ISchedulingPlan plan)
            => throw new NotImplementedException();

        private IJobDetail CreateJob(Framework.Abstraction.Services.Scheduling.IJob job)
        {
            var jobDetail = new JobDetailImpl("GenericJob", typeof(GenericJob));
            jobDetail.JobDataMap["Job"] = job;
            jobDetail.Name = job.Name;
            return jobDetail;
        }

        private ITrigger CreateTrigger(ISchedulingPlan plan, IJobDetail job)
        {
            var visitor = new SchedulingTriggerVisitor(job);
            plan.Accept(visitor);
            return visitor.Trigger;
        }
    }
}
