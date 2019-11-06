using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.ThreadManaging;
using System;
using System.Threading;

namespace Plugin.ThreadManagers
{
    internal class ManagedThread : IManagedThreadHandle
    {
        private readonly ThreadManager _manager;
        private readonly IManagedThread _runnable;
        private readonly ILogger _logger;
        private readonly Thread _thread;

        private bool _internalFinished;

        public IManagedThread Runnable => _runnable;
        public IManagedThread Thread => Runnable;
        public bool IsRunning => !_internalFinished && _thread.IsAlive;
        public bool HasFailed { get; private set; }
        public bool WasInterrupted { get; private set; }

        public ManagedThread(ThreadManager manager, IManagedThread runnable, ILogger logger)
        {
            _internalFinished = false;

            _manager = manager;
            _runnable = runnable;
            _logger = logger;
            _thread = new Thread(Run) { Name = runnable.Name };
        }

        public void Start()
        {
            if (_thread.ThreadState != ThreadState.Unstarted) throw new InvalidOperationException("Starting a thread twice is not allowed");

            _logger.Debug("Starting ManagedThread [{0}]", Runnable.Name);
            _thread.Start();
        }

        public void Interrupt()
        {
            _logger.Debug("Interrupting ManagedThread [{0}]", Runnable.Name);

            WasInterrupted = true;
            _thread.Interrupt();
        }

        private void Run()
        {
            try
            {
                _runnable.Run(this);
            }
            catch (OperationCanceledException)
            {
                _logger.Debug(string.Format("ManagedThread [{0}] was interrupted", Runnable.Name));
            }
            catch (ThreadInterruptedException)
            {
                _logger.Debug(string.Format("ManagedThread [{0}] was interrupted", Runnable.Name));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("ManagedThread [{0}] runs in error", Runnable.Name));
                HasFailed = true;
            }
            finally
            {
                _manager.MarkAsFinished(this);

                _logger.Debug(string.Format("Context Marked as finished [{0}]", Runnable.Name));
                _internalFinished = true;
                _logger.Debug(string.Format("ManagedThread [{0}] reached end of 'Run()' Method", Runnable.Name));
            }
        }
    }
}
