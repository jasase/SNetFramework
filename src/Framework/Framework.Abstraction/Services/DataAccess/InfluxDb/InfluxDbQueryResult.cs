using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Abstraction.Services.DataAccess.InfluxDb
{
    public class InfluxDbQueryResult
    {
        public string SeriesName { get; set; }

        public dynamic[] Data { get; set; }
    }
}
