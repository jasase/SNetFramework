using Framework.Abstraction.Services.DataAccess.InfluxDb;
using System;
using System.Collections.Generic;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbUploadQueueElement
    {
        private readonly List<Exception> _errors;
        public InfluxDbUploadQueueElement(InfluxDbEntry[] entries, int retryCount, string database, string retentioPolicy)
        {
            _errors = new List<Exception>();

            Entries = entries;
            RetryCount = retryCount;
            Database = database;
            RetentioPolicy = retentioPolicy;
        }

        public IReadOnlyCollection<Exception> Errors => _errors;
        public InfluxDbEntry[] Entries { get; }
        public int RetryCount { get; set; }
        public string Database { get; }
        public string RetentioPolicy { get; }

        public void AddError(Exception ex)
            => _errors.Add(ex);
    }
}
