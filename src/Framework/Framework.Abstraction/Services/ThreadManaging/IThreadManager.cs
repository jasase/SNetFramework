namespace Framework.Contracts.Services.ThreadManaging
{
    public interface IThreadManager
    {
        IManagedThreadHandle Start(IManagedThread thread);

        void StopAllThreads();

        void StopThread(IManagedThreadHandle managedThread);        
    }
}
