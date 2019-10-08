using Framework.Abstraction.Services.Configuration;
using StructureMap;

namespace ConfigurationPlugin.AlgorithmFactories
{
    public class GenericSettingAlgorithmFactory<T, TSetting> : DefaultAlgorithmFactory, IAlgorithmFactory<T, TSetting>
        where T : class
        where TSetting : IAlgorithmSetting
    {
        private readonly TSetting _AlgorithmSetting;
        private readonly IContainer _container;

        public GenericSettingAlgorithmFactory(string typeName, TSetting algorithmSetting, IContainer container)
            : base(typeName, container)
        {
            _AlgorithmSetting = algorithmSetting;
            _container = container;
        }

        public T GetAlgorithm()
        {
            return _container.GetInstance<T>(AlgorithName);
        }
    }
}
