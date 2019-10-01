namespace Framework.Abstraction.Services.Scheduling
{
    public interface IJob
    {
        string Name { get; }
        void Execute();
    }
}
