using System.Collections.Generic;

namespace ConfigurationPlugin.Configuration
{
    public class Area
    {
        public string Name { get; set; }
        public IList<AreaElement> Elements { get; private set; }

        public Area()
        {
            Name = string.Empty;
            Elements = new List<AreaElement>();
        }
    }
}
