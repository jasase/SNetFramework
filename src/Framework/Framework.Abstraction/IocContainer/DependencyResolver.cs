using System;

namespace Framework.Abstraction.IocContainer
{
    public static class DependencyResolver
    {
        private static IDependencyResolver _current = null;

        public static IDependencyResolver Instance
        {
            get
            {
                if (_current == null) throw new InvalidOperationException("DependencyResolver currently not setuped");
                return _current;
            }
        }

        public static void Setup(IDependencyResolver resolver)
        {
            if (_current != null) throw new InvalidOperationException("Dependency resolver already setuped");
            _current = resolver;
        }
    }
}
