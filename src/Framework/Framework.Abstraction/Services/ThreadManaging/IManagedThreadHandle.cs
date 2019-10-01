namespace Framework.Abstraction.Services.ThreadManaging
{
    public interface IManagedThreadHandle
    {
        IManagedThread Thread { get; }

        bool IsRunning { get; }
        bool HasFailed { get; }
        bool WasInterrupted { get; }
    }
}
