using System;
using System.Linq;

namespace Framework.Abstraction.Services.DataAccess.InfluxDb
{
    public class InfluxDbEntry
    {
        public DateTime? Time { get; set; }
        public string Measurement { get; set; }
        public InfluxDbEntryField[] Tags { get; set; }
        public InfluxDbEntryField[] Fields { get; set; }

        public override string ToString()
        {
            var date = Time ?? DateTime.MinValue;
            var tags = string.Join(", ", Tags.Select(x => x.ToString()));
            var fields = string.Join(", ", Fields.Select(x => x.ToString()));

            return string.Format("{0,-20} | {1,-50} | {2,-100} | {3}",
                                 date,
                                 Measurement,
                                 tags,
                                 fields);
        }
    }

    public class InfluxDbEntryField
    {
        public string Name { get; set; }
        public IComparable Value { get; set; }

        public override string ToString()
            => $"{Name}={Value ?? "null"}";
    }
}
