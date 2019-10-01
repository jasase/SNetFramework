using System;
using System.Collections.Generic;

namespace Framework.Abstraction.Plugins
{
    public class PluginDescription
    {
        public PluginDescription()
        {
            Name = string.Empty;
            Description = string.Empty;
            NeededServices = new Type[0];
            ProvidedServices = new Type[0];
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Type> NeededServices { get; set; }
        public IEnumerable<Type> ProvidedServices { get; set; }
    }
}
