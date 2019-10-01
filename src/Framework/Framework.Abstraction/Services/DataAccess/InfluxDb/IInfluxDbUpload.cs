namespace Framework.Contracts.Services.DataAccess.InfluxDb
{
    public interface IInfluxDbUpload
    {
        void Write(InfluxDbEntry[] entries, string database);
        void Write(InfluxDbEntry[] entries, string database, string retentionPoliciy);

        void QueueWrite(InfluxDbEntry[] entries, int maxRetries, string database);
        void QueueWrite(InfluxDbEntry[] entries, int maxRetries, string database, string retentionPoliciy);
    }
}
