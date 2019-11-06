using System;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbUploadRetryQueueElement
    {
        public DateTime AddTime { get; private set; }
        public InfluxDbUploadQueueElement Job { get; }

        public InfluxDbUploadRetryQueueElement(InfluxDbUploadQueueElement job)
        {
            AddTime = DateTime.Now;
            Job = job;
        }
    }
}
