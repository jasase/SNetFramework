using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Abstraction.Services.DataAccess.InfluxDb
{
    public class InfluxDbContinuousQueryDefinition
    {
        public string Name { get; }
        public string Query { get; }
        public TimeSpan ResampleDuration { get; }
        public TimeSpan ResampleFrequency { get; }

        public InfluxDbContinuousQueryDefinition(string name,
                                                 string query,
                                                 TimeSpan resampleDuration,
                                                 TimeSpan resampleFrequency)
        {
            Name = name;
            Query = query;
            ResampleDuration = resampleDuration;
            ResampleFrequency = resampleFrequency;
        }
    }
}
