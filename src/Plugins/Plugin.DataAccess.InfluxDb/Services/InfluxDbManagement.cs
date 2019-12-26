using System;
using System.Linq;
using AdysTech.InfluxDB.Client.Net;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.DataAccess.InfluxDb;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbManagement : IInfluxDbManagement
    {
        private InfluxDBClient _influxDbClient;
        private readonly ILogger _logger;

        public InfluxDbManagement(InfluxDBClient influxDbClient, ILogger logger)
        {
            _influxDbClient = influxDbClient;
            _logger = logger;
        }

        public bool EnsureContinuousQuery(string databaseName, InfluxDbContinuousQueryDefinition definition)
        {
            var cqs = _influxDbClient.GetContinuousQueriesAsync()
                                     .Result
                                     .FirstOrDefault(x => x.Name.Equals(definition.Name, StringComparison.InvariantCulture));
            if (cqs != null)
            {
                if (cqs.Query.Equals(definition.Query, StringComparison.InvariantCulture) &&
                   cqs.DBName.Equals(databaseName, StringComparison.InvariantCulture) &&
                   cqs.ResampleDuration.Equals(definition.ResampleDuration) &&
                   cqs.ResampleFrequency.Equals(definition.ResampleFrequency))
                {
                    _logger.Debug("Continuous query {0} already exists. Nothing to do.", definition.Name);
                    return false;
                }
                else
                {
                    _logger.Info("Continuous query {0} already exists, but with different configuration. Current CQ will be deleted and new created.",
                                  definition.Name);
                    _influxDbClient.DropContinuousQueryAsync(cqs);
                }
            }
            else
            {
                _logger.Info("Continuous query {0} does not exists. CQ will be created.",
                                  definition.Name);
            }

            var p = new InfluxContinuousQuery()
            {
                Name = definition.Name,
                DBName = databaseName,
                Query = definition.Query,
                ResampleDuration = definition.ResampleDuration,
                ResampleFrequency = definition.ResampleFrequency
            };
            return _influxDbClient.CreateContinuousQueryAsync(p).Result;
        }

        public bool EnsureDatabase(string databaseName)
        {
            var dbs = _influxDbClient.GetInfluxDBNamesAsync().Result;
            if (dbs.Any(x => x.Equals(databaseName, StringComparison.InvariantCulture)))
            {
                _logger.Debug("Database {0} already exists. Nothing to do.", databaseName);
                return false;
            }
            else
            {
                _logger.Info("Database {0} does not exists. DB will be created.", databaseName);
                return _influxDbClient.CreateDatabaseAsync(databaseName).Result;
            }
        }

        public bool EnsureRetentionPolicy(string databaseName, InfluxDbRetentionPolicyDefinition definition)
        {
            var rps = _influxDbClient.GetRetentionPoliciesAsync(databaseName)
                                     .Result
                                     .FirstOrDefault(x => x.Name.Equals(definition.Name, StringComparison.InvariantCulture)); ;
            if (rps != null)
            {
                _logger.Debug("Retention policy {0} already exists. Nothing to do.", definition.Name);
                return false;
            }

            _logger.Debug("Retention policy {0} does not exists. RP will be created.", definition.Name);
            var rp = new InfluxRetentionPolicy()
            {
                Name = definition.Name,
                DBName = databaseName,
                Duration = definition.Duration,
                IsDefault = definition.IsDefault
            };
            return _influxDbClient.CreateRetentionPolicyAsync(rp).Result;
        }
    }
}
