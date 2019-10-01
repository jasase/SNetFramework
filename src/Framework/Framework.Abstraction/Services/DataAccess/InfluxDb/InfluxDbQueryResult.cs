using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Contracts.Services.DataAccess.InfluxDb
{
    public class InfluxDbQueryResult
    {
        public string SeriesName { get; set; }

        public dynamic[] Data { get; set; }
    }
}
