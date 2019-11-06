using Framework.Abstraction.Services.DataAccess.InfluxDb;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using Framework.Abstraction.Services.ThreadManaging;
using Framework.Abstraction.Extension;
using Framework.Abstraction.IocContainer;
using Framework.Abstraction.Services.Scheduling;
using Framework.Core.Scheduling;
using System;
using AdysTech.InfluxDB.Client.Net;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbUpload : IInfluxDbUpload
    {
        private readonly IDependencyResolver _resolver;
        private readonly ILogger _logger;
        private readonly InfluxDBClient _client;
        private readonly IThreadManager _threadManager;
        private readonly ISchedulingService _schedulingService;
        private readonly BlockingCollection<InfluxDbUploadQueueElement> _jobQueue;
        private readonly List<InfluxDbUploadRetryQueueElement> _retryJobQueue;

        private IManagedThreadHandle _handle;
        private InfluxDbUploadRetryJob _retryJob;

        public InfluxDbUpload(ILogger logger,
                              InfluxDBClient client,
                              IThreadManager threadManager,
                              ISchedulingService schedulingService)
        {
            _logger = logger;
            _client = client;
            _threadManager = threadManager;
            _schedulingService = schedulingService;

            _jobQueue = new BlockingCollection<InfluxDbUploadQueueElement>();
            _retryJobQueue = new List<InfluxDbUploadRetryQueueElement>();


        }

        public void QueueWrite(InfluxDbEntry[] entries, int maxRetries)
            => QueueWrite(entries, maxRetries, null);
        public void QueueWrite(InfluxDbEntry[] entries, int maxRetries, string database)
            => QueueWrite(entries, maxRetries, database, null);
        public void QueueWrite(InfluxDbEntry[] entries, int maxRetries, string database, string retentionPoliciy)
        {
            _logger.Debug("Queuing {0} entries for influx db upload: \r\n{1}",
                          entries.Length,
                          string.Join(Environment.NewLine, entries.Select(x => x.ToString())));
            _jobQueue.Add(new InfluxDbUploadQueueElement(entries, maxRetries, database, retentionPoliciy));

            if (_handle == null || !_handle.IsRunning || _handle.HasFailed)
            {
                if (_handle != null)
                {
                    _logger.Warn("Not running upload thread detected. Starting new one. Stopping old thread. Has failed: {0}", _handle.HasFailed);
                    _threadManager.StopThread(_handle);
                }
                else
                {
                    _logger.Info("Starting first time upload thread.");
                }

                var newThread = new InfluxDbUploadThread(_jobQueue, _retryJobQueue, _logger, this);
                _handle = _threadManager.Start(newThread);
            }

            if (_retryJob == null)
            {
                _logger.Info("Starting first time retry job.");
                _retryJob = new InfluxDbUploadRetryJob(_logger, _jobQueue, _retryJobQueue);
                _schedulingService.AddJob(_retryJob, new PollingPlan(TimeSpan.FromMinutes(1)));
            }
        }

        public void Write(InfluxDbEntry[] entries)
            => Write(entries, null, null);

        public void Write(InfluxDbEntry[] entries, string database)
            => Write(entries, database, null);

        public void Write(InfluxDbEntry[] entries, string database, string retentionPoliciy)
            => _client.PostPointsAsync(database, ConvertEntries(entries, retentionPoliciy)).Wait();

        private IEnumerable<IInfluxDatapoint> ConvertEntries(InfluxDbEntry[] entries, string retentionPoliciy)
        {
            foreach (var entry in entries)
            {
                var point = new InfluxDatapoint<InfluxValueField>();
                point.MeasurementName = entry.Measurement;

                if (entry.Time.HasValue)
                {
                    point.UtcTimestamp = entry.Time.Value;
                }

                foreach (var tag in entry.Tags)
                {
                    point.Tags.Add(tag.Name, tag.Value.ToString());
                }

                foreach (var value in entry.Fields)
                {
                    if (value.Value is IComparable)
                    {
                        point.Fields.Add(value.Name, new InfluxValueField((IComparable) value.Value));
                    }
                    else
                    {
                        _logger.Error("Value for InfluxDB write provided that could not be uploaded");
                    }
                }

                if (!string.IsNullOrEmpty(retentionPoliciy))
                {
                    point.Retention = new InfluxRetentionPolicy() { Name = retentionPoliciy };
                }

                yield return point;
            }
        }
    }
}
