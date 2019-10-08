using Framework.Abstraction.Services.Configuration;
using StructureMap;

namespace ConfigurationPlugin.AlgorithmFactories
{
    public class GenericAlgorithmFactory<T> : DefaultAlgorithmFactory, IAlgorithmFactory<T>
        where T :class
    {
        private readonly IContainer _container;

        public GenericAlgorithmFactory(string typeName, IContainer container) 
            : base(typeName, container)
        {
            _container = container;
        }

        public T GetAlgorithm()
        {
            return _container.GetInstance<T>(AlgorithName);
        }
    }
}
