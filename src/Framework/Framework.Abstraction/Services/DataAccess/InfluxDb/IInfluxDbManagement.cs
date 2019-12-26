namespace Framework.Abstraction.Services.DataAccess.InfluxDb
{
    public interface IInfluxDbManagement
    {
        public bool EnsureDatabase(string databaseName);
        public bool EnsureContinuousQuery(string databaseName, InfluxDbContinuousQueryDefinition definition);
        public bool EnsureRetentionPolicy(string databaseName, InfluxDbRetentionPolicyDefinition definition);
    }
}
