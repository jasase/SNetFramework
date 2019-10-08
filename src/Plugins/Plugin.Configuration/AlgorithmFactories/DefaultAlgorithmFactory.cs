using Framework.Abstraction.Services.Configuration;
using StructureMap;

namespace ConfigurationPlugin.AlgorithmFactories
{
    public class DefaultAlgorithmFactory : IAlgorithmFactory
    {
        private readonly IContainer _container;

        public string AlgorithName { get; private set; }               

        public DefaultAlgorithmFactory(string typeName, IContainer container)
        {
            AlgorithName = typeName;
            _container = container;
        }

        public T GetAlgorithm<T>()
        {            
            return _container.GetInstance<T>(AlgorithName);
        }
    }
}
