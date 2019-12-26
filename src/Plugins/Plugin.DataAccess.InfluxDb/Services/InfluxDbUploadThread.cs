using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.DataAccess.InfluxDb;
using Framework.Abstraction.Services.ThreadManaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbUploadThread : IManagedThread
    {
        private readonly BlockingCollection<InfluxDbUploadQueueElement> _jobQueue;
        private readonly List<InfluxDbUploadRetryQueueElement> _retryJobQueue;
        private readonly ILogger _logger;
        private readonly IInfluxDbUpload _upload;

        public string Name => "InfluxDbUpload";

        public InfluxDbUploadThread(BlockingCollection<InfluxDbUploadQueueElement> jobQueue,
                                    List<InfluxDbUploadRetryQueueElement> retryJobQueue,
                                    ILogger logger,
                                    IInfluxDbUpload upload)
        {
            _jobQueue = jobQueue;
            _retryJobQueue = retryJobQueue;
            _logger = logger;
            _upload = upload;
        }

        public void Run(IManagedThreadHandle handle)
        {
            while (!handle.WasInterrupted)
            {
                try
                {
                    foreach (var job in _jobQueue.GetConsumingEnumerable())
                    {
                        if (job.RetryCount < 1)
                        {
                            _logger.Error("Job has reached max retry count. Discarding job. Errors are:{0}",
                                          string.Join(Environment.NewLine, job.Errors.Select(x => x.ToString())));
                        }
                        try
                        {
                            _logger.Debug("Uploading entries");
                            if (string.IsNullOrEmpty(job.RetentioPolicy))
                            {
                                _upload.Write(job.Entries, job.Database);
                            }
                            else
                            {
                                _upload.Write(job.Entries, job.Database, job.RetentioPolicy);
                            }
                            _logger.Info("Uploading entries done");
                        }
                        catch (Exception ex)
                        {
                            _logger.Warn("Uploading of entries from job run on error. Current count of retries is {0}", job.RetryCount);
                            job.RetryCount--;
                            job.AddError(ex);

                            lock (_retryJobQueue)
                            {
                                _retryJobQueue.Add(new InfluxDbUploadRetryQueueElement(job));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "In influxDbUpload thread an error occured");
                }
            }
        }
    }
}
