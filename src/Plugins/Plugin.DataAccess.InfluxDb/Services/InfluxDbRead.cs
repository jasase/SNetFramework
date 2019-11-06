using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdysTech.InfluxDB.Client.Net;
using Framework.Abstraction.Services.DataAccess.InfluxDb;

namespace Plugin.DataAccess.InfluxDb.Services
{
    public class InfluxDbRead : IInfluxDbRead
    {
        private readonly InfluxDBClient _client;

        public InfluxDbRead(InfluxDBClient client)
        {
            _client = client;
        }

        IEnumerable<InfluxDbQueryResult> IInfluxDbRead.Read(string database, string query)
        {
            var result = _client.QueryMultiSeriesAsync(database, query).Result;

            foreach (var cur in result.Where(x => x.HasEntries))
            {
                yield return new InfluxDbQueryResult
                {
                    SeriesName = cur.SeriesName,
                    Data = cur.Entries.ToArray()
                };
            }
        }
    }
}
