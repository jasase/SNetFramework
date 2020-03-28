using System;

namespace Framework.Abstraction.Extension
{
    public abstract class MetricIdentifier
    {
        protected MetricIdentifier(string regionName, string identifier)
        {
            RegionName = (regionName ?? string.Empty).ToUpperInvariant();
            Identifier = (identifier ?? string.Empty).ToUpperInvariant();
        }

        public string RegionName { get; }
        public string Identifier { get; }

        public override string ToString()
            => RegionName + ":" + Identifier;

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null ||
                GetType() != obj.GetType())
            {
                return false;
            }

            var other = (MetricIdentifier) obj;
            return RegionName.Equals(other.RegionName, StringComparison.InvariantCultureIgnoreCase) &&
                   Identifier.Equals(other.Identifier, StringComparison.InvariantCultureIgnoreCase);
        }

        // override object.GetHashCode
        public override int GetHashCode()
            => RegionName.GetHashCode(StringComparison.InvariantCultureIgnoreCase) ^
               Identifier.GetHashCode(StringComparison.InvariantCultureIgnoreCase) ^
               834678;
    }
}
