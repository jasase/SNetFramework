namespace Framework.Abstraction.Services.Scheduling
{
    public interface ISchedulingPlan
    {
        void Accept(ISchedulingPlanVisitor visitor);
    }
}
