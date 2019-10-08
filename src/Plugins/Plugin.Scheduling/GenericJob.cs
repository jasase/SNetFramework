using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Quartz;
using System;
using System.Threading.Tasks;

namespace QuartzPlugin
{
    public class GenericJob : IJob
    {
        internal const string JOB_KEY = "Job";
        private readonly ILogger _logger;

        public GenericJob()
        {
            _logger = DependencyResolver.Instance.GetInstance<ILogManager>().GetLogger(GetType());
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.Debug("Job {0} was triggerd", context.JobDetail.Key.Name);
            try
            {
                if (context.JobDetail.JobDataMap.Contains(JOB_KEY) &&
                    context.JobDetail.JobDataMap[JOB_KEY] is Framework.Abstraction.Services.Scheduling.IJob)
                {
                    //TODO Create Context
                    ((Framework.Abstraction.Services.Scheduling.IJob) context.JobDetail.JobDataMap[JOB_KEY]).Execute();
                    _logger.Debug("Job {0} was executed successfully", context.JobDetail.Key.Name);
                }
                else
                {
                    _logger.Error("Quartz Context doesn't contains IJOB instances for job {0}. Aborting execution of Job", context.JobDetail.Key.Name);
                    await context.Scheduler.DeleteJob(context.JobDetail.Key);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "During execution of job {0} ocurred an error", context.JobDetail.Key.Name);
            }
        }        
    }
}
