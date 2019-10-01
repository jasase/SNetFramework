namespace Framework.Contracts.Services.Scheduling
{
    public interface ISchedulingPlanVisitor
    {
        void Handle(IWeekleyPlan plan);
        void Handle(IPollingPlan plan);
        void Handle(IOneTimePlan plan);
        void Handle(IDailyPlan plan);
    }
}
