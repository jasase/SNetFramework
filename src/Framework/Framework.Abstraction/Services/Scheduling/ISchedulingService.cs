using System;

namespace Framework.Abstraction.Services.Scheduling
{
    public interface ISchedulingService
    {        
        Tuple<IJob, ISchedulingPlan> ActiveSchedulings { get; }
        
        void AddJob(IJob job, ISchedulingPlan plan);
        void RemoveJob(IJob job, ISchedulingPlan plan);
    }
}
