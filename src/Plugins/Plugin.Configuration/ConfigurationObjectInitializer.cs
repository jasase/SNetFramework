using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ConfigurationPlugin.AlgorithmFactories;
using ConfigurationPlugin.Configuration;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Services.Configuration;
using IContainer = StructureMap.IContainer;

namespace ConfigurationPlugin
{
    public class ConfigurationObjectInitializer
    {
        private const string SettingSuffix1 = "Setting";
        private const string SettingSuffix2 = "Settings";

        private readonly IContainer _Container;
        private readonly ILogger _Logger;

        public ConfigurationObjectInitializer(IContainer container, ILogger logger)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (logger == null) throw new ArgumentNullException("logger");
            _Container = container;
            _Logger = logger;
        }

        public void Initialice(List<Area> values)
        {
            var settings = _Container.GetAllInstances<ISetting>().ToArray();
            var possibleAreas = new List<string>(settings.Length);
            var valueDir = values.ToDictionary(x => x.Name);
            foreach (var settingObject in settings)
            {
                InitialiceSettingObject(valueDir, settingObject, possibleAreas);
                _Container.Configure(x => x.ForSingletonOf(settingObject.GetType()).Use(settingObject));
            }
            if (valueDir.Any())
            {
                _Logger.Warn("[{0}] are not possible areas. Possible areas are [{1}]",
                    string.Join(",", values.Select(x => x.Name)),
                    string.Join(",", possibleAreas));
            }
        }

        private void InitialiceSettingObject(Dictionary<string, Area> values, ISetting settingObject, List<string> possibleAreas)
        {
            var settingType = settingObject.GetType();
            var settingName = settingType.Name;

            if (MatchSettings(ref settingName))
            {
                possibleAreas.Add(settingName);
                if (values.ContainsKey(settingName))
                {
                    var elementDir = values[settingName].Elements.ToDictionary(x => x.Key);
                    var properties = settingType.GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        if (!elementDir.ContainsKey(propertyInfo.Name)) continue;
                        var visitor = new AreaElementHandlerVisitor(propertyInfo, settingObject, _Logger, values[settingName], _Container);
                        elementDir[propertyInfo.Name].Accept(visitor);
                        elementDir.Remove(propertyInfo.Name);
                    }

                    if (elementDir.Any())
                    {
                        _Logger.Warn("[{0}] are not possible Settings for area [{1}]. Possible Settings are [{2}]",
                                     string.Join(",", elementDir.Select(x => x.Key)),
                                     settingName,
                                     string.Join(",", properties.Select(x => x.Name)));
                    }

                    _Container.Configure(x => x.For(settingType).Singleton().Use(settingObject));
                    values.Remove(settingName);
                }
            }
            else
            {
                _Logger.Warn("Class with name [{0}] not matching naming rule <Name>Setting", settingName);
            }
        }

        private bool MatchSettings(ref string settingsName)
        {
            if (settingsName.EndsWith(SettingSuffix1))
            {
                settingsName = settingsName.Substring(0, settingsName.Length - SettingSuffix1.Length);
                return true;
            }
            if (settingsName.EndsWith(SettingSuffix2))
            {
                settingsName = settingsName.Substring(0, settingsName.Length - SettingSuffix2.Length);
                return true;
            }
            return false;
        }

        private class AreaElementHandlerVisitor : IAreaElementVisitor
        {
            private readonly PropertyInfo _Property;
            private readonly ISetting _SettingObject;
            private readonly ILogger _Logger;
            private readonly Area _Area;
            private readonly IContainer _Container;

            public AreaElementHandlerVisitor(PropertyInfo property, ISetting settingObject, ILogger logger, Area area, IContainer container)
            {
                _Property = property;
                _SettingObject = settingObject;
                _Logger = logger;
                _Area = area;
                _Container = container;
            }

            public void Handle(Algorithm setting)
            {
                var t = _Property.PropertyType;
                if (typeof(IAlgorithmFactory).IsAssignableFrom(t))
                {
                    IAlgorithmFactory factory;
                    if (t.IsGenericType && typeof(IAlgorithmFactory<,>).IsAssignableFrom(t.GetGenericTypeDefinition()))
                    {
                        var arguments = t.GetGenericArguments();
                        var algorithmSetting = CreateAlgorithmSettings(setting.Settings, arguments[1], setting);
                        factory = (IAlgorithmFactory)Activator.CreateInstance(typeof(GenericSettingAlgorithmFactory<,>).MakeGenericType(arguments), setting.Name, algorithmSetting, _Container);
                        _Container.Configure(x => x.For(arguments[1]).Singleton().Use(algorithmSetting));
                    }
                    else if (t.IsGenericType && typeof(IAlgorithmFactory<>).IsAssignableFrom(t.GetGenericTypeDefinition()))
                    {
                        var arguments = t.GetGenericArguments();
                        factory = (IAlgorithmFactory)Activator.CreateInstance(typeof(GenericAlgorithmFactory<>).MakeGenericType(arguments), setting.Name, _Container);
                    }
                    else
                    {
                        factory = new DefaultAlgorithmFactory(setting.Name, _Container);
                    }
                    _Property.SetMethod.Invoke(_SettingObject, new[] { factory });
                }
                else
                {
                    _Logger.Error("Setting [{0}] in area [{1}] is not a property for an algorithm. Expected type for this setting is [{2}]",
                                    _Property.Name,
                                    _Area.Name,
                                    _Property.PropertyType.Name);
                }
            }

            private IAlgorithmSetting CreateAlgorithmSettings(List<Setting> settings, Type type, Algorithm algorithm)
            {
                var algorithmSetting = (IAlgorithmSetting)Activator.CreateInstance(type);
                var settingsDir = settings.ToDictionary(x => x.Key);
                var possibleSettingProperties = new List<string>();
                foreach (var properties in type.GetProperties())
                {
                    if (settingsDir.ContainsKey(properties.Name))
                    {
                        ConvertWithFailureMessage(properties, algorithmSetting, settingsDir[properties.Name].Value,
                  string.Format("Setting [{0}] of algorithm[{3}] in area [{1}] can't converted to type [{2}]",
                                _Property.Name,
                                _Area.Name,
                                _Property.PropertyType.Name,
                                algorithm.Key),
                  string.Format("Setting the value for setting [{0}]  of algorithm[{2}] in area [{1}] was not possible",
                                    _Property.Name,
                                    _Area.Name,
                                    algorithm.Key));
                        settingsDir.Remove(properties.Name);
                    }
                    possibleSettingProperties.Add(properties.Name);
                }
                if (settingsDir.Any())
                {
                    _Logger.Warn("[{0}] are not possible settings for algorithm [{1}] in area [{2}]. Possible areas are [{3}]",
                    string.Join(",", settingsDir.Select(x => x.Key)),
                    algorithm.Key,
                    _Area.Name,
                    string.Join(",", possibleSettingProperties));
                }

                return algorithmSetting;
            }

            public void Handle(Setting setting)
            {
                ConvertWithFailureMessage(_Property, _SettingObject, setting.Value,
                    string.Format("Setting [{0}] in area [{1}] can't converted to type [{2}]",
                                  _Property.Name,
                                  _Area.Name,
                                  _Property.PropertyType.Name),
                    string.Format("Setting the value for setting [{0}] in area [{1}] was not possible",
                                      _Property.Name,
                                      setting.Key));
            }

            private void ConvertWithFailureMessage(PropertyInfo property, object objectToSet, string newValue, string notConvertableMessage, string failureDuringConvert)
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        var convertedValue = converter.ConvertFromString(newValue);
                        property.SetMethod.Invoke(objectToSet, new[] { convertedValue });
                    }
                    catch (Exception ex)
                    {
                        _Logger.Error(ex, failureDuringConvert);
                    }
                }
                else
                {
                    _Logger.Error(notConvertableMessage);
                }
            }
        }
    }
}
