using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.Scheduling;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbUploadRetryJob : IJob
    {
        private readonly ILogger _logger;
        private readonly BlockingCollection<InfluxDbUploadQueueElement> _jobQueue;
        private readonly List<InfluxDbUploadRetryQueueElement> _retryJobQueue;

        public string Name => "InfluxDbUploadRetryJob";

        public InfluxDbUploadRetryJob(ILogger logger,
                                      BlockingCollection<InfluxDbUploadQueueElement> jobQueue,
                                      List<InfluxDbUploadRetryQueueElement> retryJobQueue)
        {
            _logger = logger;
            _jobQueue = jobQueue;
            _retryJobQueue = retryJobQueue;
        }

        public void Execute()
        {
            var now = DateTime.Now;
            var waitTime = TimeSpan.FromMinutes(5);

            lock (_retryJobQueue)
            {
                if (_retryJobQueue.Any())
                {
                    var filtered = _retryJobQueue.Where(x => now - x.AddTime > waitTime).ToArray();
                    _logger.Info("Found {0} jobs that are needed to retry", filtered.Length);
                    foreach (var job in filtered)
                    {
                        foreach (var entryWithoutTime in job.Job.Entries.Where(x => x.Time == null))
                        {
                            entryWithoutTime.Time = job.AddTime;
                        }

                        _retryJobQueue.Remove(job);
                        _jobQueue.Add(job.Job);
                    }
                }
            }
        }
    }
}
