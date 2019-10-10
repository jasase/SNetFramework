using System;
using System.Collections.Generic;
using System.IO;
using ConfigurationPlugin.Configuration;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.XmlToObject;
using Framework.Abstraction.Services;
using Plugin.Configuration;

namespace Plugin.Configuration.Loaders
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        private const string CONFIG_FILENAME = "config.xml";

        private readonly IXmlToObject _xmlToObject;
        private readonly ILogger _logger;
        private readonly IEnvironmentParameters _parameter;

        public ConfigurationLoader(IXmlToObject xmlToObject, ILogger logger, IEnvironmentParameters parameter)
        {
            _xmlToObject = xmlToObject ?? throw new ArgumentNullException(nameof(xmlToObject));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _parameter = parameter;
        }

        public IEnumerable<Area> LoadConfiguration()
        {
            var configPath = Path.Combine(_parameter.ConfigurationDirectory.FullName, CONFIG_FILENAME);
            if (File.Exists(configPath))
            {
                configuration configuration = null;

                try
                {
                    _logger.Info("Loading configuration from path '{0}'", configPath);
                    configuration = _xmlToObject.ReadXml<configuration>(configPath);
                }
                catch (Exception excpetion)
                {
                    _logger.Error(excpetion, "Error during parsing configurationfile at [{0}]", configPath);
                }

                if (configuration != null)
                {
                    var areaKeys = new List<string>(configuration.Items.Length);
                    foreach (var area in configuration.Items)
                    {
                        if (areaKeys.Contains(area.name))
                        {
                            _logger.Error("Configuration [{0}] contains more than one area with name [{1}]. Area will be ignored", configPath, area.name);
                            continue;
                        }
                        areaKeys.Add(area.name);
                        var areaElementKeys = new List<string>(area.Items.Length);
                        var areaObject = new Area()
                        {
                            Name = area.name
                        };
                        foreach (var item in area.Items)
                        {
                            var algorithm = item as algorithm;
                            if (algorithm != null)
                            {
                                if (HandleAlgorithmObject(areaElementKeys, algorithm, configPath, area, areaObject)) continue;
                            }

                            var setting = item as setting;
                            if (setting != null)
                            {
                                HandleSettingObject(areaElementKeys, setting, configPath, area, areaObject);
                            }
                        }
                        yield return areaObject;
                    }
                }
            }
            else
            {
                _logger.Info("No configurationfile found at location [{0}]", configPath);
            }
        }

        private void HandleSettingObject(List<string> areaElementKeys, setting setting, string configPath, configurationArea area,
                                         Area areaObject)
        {
            if (CheckElementExists(areaElementKeys, setting.key, configPath, area))
                return;
            areaObject.Elements.Add(
                new Setting
                {
                    Value = setting.value,
                    Key = setting.key
                });
        }

        private bool HandleAlgorithmObject(List<string> areaElementKeys, algorithm algorithm, string configPath, configurationArea area,
                                           Area areaObject)
        {
            if (CheckElementExists(areaElementKeys, algorithm.key, configPath, area))
                return true;
            var algorithmKeyList = new List<string>(algorithm.setting.Length);
            var algorithmObject = new Algorithm
            {
                Name = algorithm.name,
                Key = algorithm.key
            };
            foreach (var algorithmSetting in algorithm.setting)
            {
                if (!algorithmKeyList.Contains(algorithmSetting.key))
                {
                    algorithmObject.Settings.Add(
                        new Setting()
                        {
                            Key = algorithmSetting.key,
                            Value = algorithmSetting.value
                        });
                }
            }
            areaObject.Elements.Add(algorithmObject);
            return false;
        }

        private bool CheckElementExists(List<string> areaElementKeys, string elementName, string configPath,
                              configurationArea area)
        {
            if (areaElementKeys.Contains(elementName))
            {
                _logger.Error(
                    "Configuration [{0}] contains more than one element in area [{1}] with name [{2}]. Element will be ignored",
                    configPath, area.name, elementName);
                return true;
            }
            areaElementKeys.Add(elementName);
            return false;
        }
    }
}
