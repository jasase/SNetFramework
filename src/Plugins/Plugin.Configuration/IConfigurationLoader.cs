using System;
using System.Collections.Generic;
using System.Text;
using ConfigurationPlugin.Configuration;

namespace Plugin.Configuration
{
    public interface IConfigurationLoader
    {
        IEnumerable<Area> LoadConfiguration();
    }
}
