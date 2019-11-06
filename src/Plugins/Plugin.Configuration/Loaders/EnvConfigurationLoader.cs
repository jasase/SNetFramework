using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConfigurationPlugin.Configuration;
using Microsoft.Extensions.Configuration;

namespace Plugin.Configuration.Loaders
{
    public class EnvConfigurationLoader : IConfigurationLoader
    {
        public IEnumerable<Area> LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder().AddEnvironmentVariables()
                                                    .Build();

            var regex = new Regex(@"^([a-zA-Z]+)_([a-zA-Z]+)$");

            var configAreas = from c in configuration.AsEnumerable()
                              let r = regex.Match(c.Key)
                              where r.Success
                              let t = new { Area = r.Groups[1].Value, Key = r.Groups[2].Value, Value = c.Value }
                              group t by t.Area into g
                              select g;

            foreach (var item in configAreas)
            {
                var area = new Area()
                {
                    Name = item.Key,        
                };

                foreach (var settings in item)
                {
                    area.Elements.Add(new Setting
                    {
                        Key = settings.Key,
                        Value = settings.Value
                    });
                }

                yield return area;
            }
        }
    }
}
