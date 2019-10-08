using System;
using System.Collections.Generic;
using System.IO;
using ConfigurationPlugin.Configuration;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.XmlToObject;
using Framework.Abstraction.Services;

namespace ConfigurationPlugin
{
    public class ConfigurationLoader
    {
        private readonly IXmlToObject _XmlToObject;
        private readonly ILogger _Logger;
        private readonly IEnvironmentParameters _parameter;
        private const string ConfigFilename = "config.xml";

        public ConfigurationLoader(IXmlToObject xmlToObject, ILogger logger, IEnvironmentParameters parameter)
        {
            if (xmlToObject == null) throw new ArgumentNullException("xmlToObject");
            if (logger == null) throw new ArgumentNullException("logger");
            _XmlToObject = xmlToObject;
            _Logger = logger;
            _parameter = parameter;
        }

        public IEnumerable<Area> LoadConfiguration()
        {
            var configPath = Path.Combine(_parameter.ConfigurationDirectory.FullName, ConfigFilename);
            if (File.Exists(configPath))
            {
                configuration configuration = null;

                try
                {
                    _Logger.Info("Loading configuration from path '{0}'", configPath);
                    configuration = _XmlToObject.ReadXml<configuration>(configPath);
                }
                catch (Exception excpetion)
                {
                    _Logger.Error(excpetion, "Error during parsing configurationfile at [{0}]", configPath);
                }

                if (configuration != null)
                {
                    var areaKeys = new List<string>(configuration.Items.Length);
                    foreach (var area in configuration.Items)
                    {
                        if (areaKeys.Contains(area.name))
                        {
                            _Logger.Error("Configuration [{0}] contains more than one area with name [{1}]. Area will be ignored", configPath, area.name);
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
                _Logger.Info("No configurationfile found at location [{0}]", configPath);
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
                _Logger.Error(
                    "Configuration [{0}] contains more than one element in area [{1}] with name [{2}]. Element will be ignored",
                    configPath, area.name, elementName);
                return true;
            }
            areaElementKeys.Add(elementName);
            return false;
        }
    }
}
