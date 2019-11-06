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
        private const string SETTING_SUFFIX_1 = "Setting";
        private const string SETTING_SUFFIX_2 = "Settings";

        private readonly IContainer _container;
        private readonly ILogger _logger;

        public ConfigurationObjectInitializer(IContainer container, ILogger logger)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Initialice(List<Area> values)
        {
            var settings = _container.GetAllInstances<ISetting>().ToArray();
            var possibleAreas = new List<string>(settings.Length);
            var valueDir = values.ToDictionary(x => x.Name);
            foreach (var settingObject in settings)
            {
                InitialiceSettingObject(valueDir, settingObject, possibleAreas);
                _container.Configure(x => x.ForSingletonOf(settingObject.GetType()).Use(settingObject));
            }
            if (valueDir.Any())
            {
                _logger.Warn("[{0}] are not possible areas. Possible areas are [{1}]",
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
                        var visitor = new AreaElementHandlerVisitor(propertyInfo, settingObject, _logger, values[settingName], _container);
                        elementDir[propertyInfo.Name].Accept(visitor);
                        elementDir.Remove(propertyInfo.Name);
                    }

                    if (elementDir.Any())
                    {
                        _logger.Warn("[{0}] are not possible Settings for area [{1}]. Possible Settings are [{2}]",
                                     string.Join(",", elementDir.Select(x => x.Key)),
                                     settingName,
                                     string.Join(",", properties.Select(x => x.Name)));
                    }

                    _container.Configure(x => x.For(settingType).Singleton().Use(settingObject));
                    values.Remove(settingName);
                }
            }
            else
            {
                _logger.Warn("Class with name [{0}] not matching naming rule <Name>Setting", settingName);
            }
        }

        private bool MatchSettings(ref string settingsName)
        {
            if (settingsName.EndsWith(SETTING_SUFFIX_1, StringComparison.InvariantCultureIgnoreCase))
            {
                settingsName = settingsName.Substring(0, settingsName.Length - SETTING_SUFFIX_1.Length);
                return true;
            }
            if (settingsName.EndsWith(SETTING_SUFFIX_2, StringComparison.InvariantCultureIgnoreCase))
            {
                settingsName = settingsName.Substring(0, settingsName.Length - SETTING_SUFFIX_2.Length);
                return true;
            }
            return false;
        }

        private class AreaElementHandlerVisitor : IAreaElementVisitor
        {
            private readonly PropertyInfo _property;
            private readonly ISetting _settingObject;
            private readonly ILogger _logger;
            private readonly Area _area;
            private readonly IContainer _container;

            public AreaElementHandlerVisitor(PropertyInfo property, ISetting settingObject, ILogger logger, Area area, IContainer container)
            {
                _property = property;
                _settingObject = settingObject;
                _logger = logger;
                _area = area;
                _container = container;
            }

            public void Handle(Algorithm setting)
            {
                var t = _property.PropertyType;
                if (typeof(IAlgorithmFactory).IsAssignableFrom(t))
                {
                    IAlgorithmFactory factory;
                    if (t.IsGenericType && typeof(IAlgorithmFactory<,>).IsAssignableFrom(t.GetGenericTypeDefinition()))
                    {
                        var arguments = t.GetGenericArguments();
                        var algorithmSetting = CreateAlgorithmSettings(setting.Settings, arguments[1], setting);
                        factory = (IAlgorithmFactory) Activator.CreateInstance(typeof(GenericSettingAlgorithmFactory<,>).MakeGenericType(arguments), setting.Name, algorithmSetting, _container);
                        _container.Configure(x => x.For(arguments[1]).Singleton().Use(algorithmSetting));
                    }
                    else if (t.IsGenericType && typeof(IAlgorithmFactory<>).IsAssignableFrom(t.GetGenericTypeDefinition()))
                    {
                        var arguments = t.GetGenericArguments();
                        factory = (IAlgorithmFactory) Activator.CreateInstance(typeof(GenericAlgorithmFactory<>).MakeGenericType(arguments), setting.Name, _container);
                    }
                    else
                    {
                        factory = new DefaultAlgorithmFactory(setting.Name, _container);
                    }
                    _property.SetMethod.Invoke(_settingObject, new[] { factory });
                }
                else
                {
                    _logger.Error("Setting [{0}] in area [{1}] is not a property for an algorithm. Expected type for this setting is [{2}]",
                                    _property.Name,
                                    _area.Name,
                                    _property.PropertyType.Name);
                }
            }

            private IAlgorithmSetting CreateAlgorithmSettings(List<Setting> settings, Type type, Algorithm algorithm)
            {
                var algorithmSetting = (IAlgorithmSetting) Activator.CreateInstance(type);
                var settingsDir = settings.ToDictionary(x => x.Key);
                var possibleSettingProperties = new List<string>();
                foreach (var properties in type.GetProperties())
                {
                    if (settingsDir.ContainsKey(properties.Name))
                    {
                        ConvertWithFailureMessage(properties, algorithmSetting, settingsDir[properties.Name].Value,
                  string.Format("Setting [{0}] of algorithm[{3}] in area [{1}] can't converted to type [{2}]",
                                _property.Name,
                                _area.Name,
                                _property.PropertyType.Name,
                                algorithm.Key),
                  string.Format("Setting the value for setting [{0}]  of algorithm[{2}] in area [{1}] was not possible",
                                    _property.Name,
                                    _area.Name,
                                    algorithm.Key));
                        settingsDir.Remove(properties.Name);
                    }
                    possibleSettingProperties.Add(properties.Name);
                }
                if (settingsDir.Any())
                {
                    _logger.Warn("[{0}] are not possible settings for algorithm [{1}] in area [{2}]. Possible areas are [{3}]",
                    string.Join(",", settingsDir.Select(x => x.Key)),
                    algorithm.Key,
                    _area.Name,
                    string.Join(",", possibleSettingProperties));
                }

                return algorithmSetting;
            }

            public void Handle(Setting setting)
                => ConvertWithFailureMessage(_property, _settingObject, setting.Value,
                    string.Format("Setting [{0}] in area [{1}] can't converted to type [{2}]",
                                  _property.Name,
                                  _area.Name,
                                  _property.PropertyType.Name),
                    string.Format("Setting the value for setting [{0}] in area [{1}] was not possible",
                                      _property.Name,
                                      setting.Key));

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
                        _logger.Error(ex, failureDuringConvert);
                    }
                }
                else
                {
                    _logger.Error(notConvertableMessage);
                }
            }
        }
    }
}
