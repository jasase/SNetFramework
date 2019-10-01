namespace Framework.Contracts.Services.Scheduling
{
    public interface IJob
    {
        string Name { get; }
        void Execute();
    }
}
