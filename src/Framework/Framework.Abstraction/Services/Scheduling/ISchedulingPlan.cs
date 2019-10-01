namespace Framework.Contracts.Services.Scheduling
{
    public interface ISchedulingPlan
    {
        void Accept(ISchedulingPlanVisitor visitor);
    }
}
