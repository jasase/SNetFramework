using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Abstraction.Services.DataAccess.InfluxDb
{
    public class InfluxDbRetentionPolicyDefinition
    {
        public InfluxDbRetentionPolicyDefinition(string name, TimeSpan duration, bool isDefault)
        {
            Name = name;
            Duration = duration;
            IsDefault = isDefault;
        }

        public string Name { get; }
        public TimeSpan Duration { get; }
        public bool IsDefault { get; }
    }
}
