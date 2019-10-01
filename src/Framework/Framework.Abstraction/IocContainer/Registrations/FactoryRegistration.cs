using System;

namespace Framework.Abstraction.IocContainer
{
    public abstract class FactoryRegistration : DependencyResolverRegistration
    {
        public abstract Func<object, FactoryContext, object> FactoryMethod { get; }
        public abstract Type FactoryType { get; }

        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
        {
            visitor.Handle(this);
        }
    }

    public class FactoryRegistration<TInterface, TFactory> : FactoryRegistration
    {
        private readonly Func<object, FactoryContext, object> _factoryMethod;

        public FactoryRegistration(Func<TFactory, FactoryContext, TInterface> factoryMethod)
        {
            _factoryMethod = (factory, context) =>
            {
                if (factory is TFactory)
                {
                    return factoryMethod((TFactory)factory, context);
                }
                throw new InvalidOperationException(
                    string.Format("Factory for creating dependency '{0}' has not the expected type '{1}'. Current type is '{2}'",
                    typeof(TInterface).FullName,
                    typeof(TFactory).FullName,
                    factory.GetType().FullName));
            };
        }

        public override Type InterfaceType { get { return typeof(TInterface); } }
        public override Func<object, FactoryContext, object> FactoryMethod { get { return _factoryMethod; } }
        public override Type FactoryType { get { return typeof(TFactory); } }
    }

    public class FactoryContext
    {
        public Type ParentType { get; set; }
        public string RequestedName { get; set; }
    }
}