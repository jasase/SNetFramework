using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Messages;
using Framework.Abstraction.Services.ThreadManaging;

namespace Plugin.ThreadManagers
{
    public class ThreadManager : IThreadManager, IMessageReceiver<SystemIsShutingDownMessage>
    {
        private readonly object _lock;
        private readonly ILogger _logger;
        private HashSet<ManagedThread> _threads;

        public ThreadManager(ILogger logger, IEventService eventService)
        {
            eventService.Register(this);

            _threads = new HashSet<ManagedThread>();
            _logger = logger;
            _lock = new object();
        }


        public IManagedThreadHandle Start(IManagedThread thread)
        {
            lock (_lock)
            {
                if (_threads.Any(x => x.Runnable == thread))
                {
                    throw new InvalidOperationException("Runnable already running");
                }

                var newThread = new ManagedThread(this, thread, _logger);
                newThread.Start();
                _threads.Add(newThread);
                return newThread;
            }
        }

        public void StopAllThreads()
        {
            lock (_lock)
            {
                var curThreads = _threads.ToArray();
                foreach (var cur in curThreads)
                {
                    StopThread(cur);
                }
            }
        }

        public void StopThread(IManagedThreadHandle managedThread)
        {
            lock (_lock)
            {
                var curThread = _threads.FirstOrDefault(x => x == managedThread);
                if (curThread != null)
                {
                    StopThread(curThread);
                }
            }
        }

        public void Update(SystemIsShutingDownMessage message)
            => StopAllThreads();

        private void StopThread(ManagedThread thread)
            => thread.Interrupt();

        internal void MarkAsFinished(ManagedThread thread)
        {
            lock (_lock)
            {
                _threads.Remove(thread);
            }
        }

        
    }
}
