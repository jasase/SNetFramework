using System;

namespace Framework.Contracts.IocContainer
{
    public abstract class NamedServiceRegistration : DependencyResolverRegistration
    {
        public abstract Type ServiceType { get; }
        public string Name { get; private set; }

        public NamedServiceRegistration(string name)
        {
            Name = name;
        }

        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
        {
            visitor.Handle(this);
        }
    }

    public class NamedServiceRegistration<TInterface, TService> : NamedServiceRegistration
        where TService : class, TInterface
    {
        public override Type ServiceType { get { return typeof(TService); } }

        public override Type InterfaceType { get { return typeof(TInterface); } }

        public NamedServiceRegistration(string name)
            : base(name)
        { }

    }
}