using System;

namespace Framework.Contracts.IocContainer
{
    public abstract class ServiceRegistration : DependencyResolverRegistration
    {
        public abstract Type ServiceType { get; }

        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
        {
            visitor.Handle(this);
        }
    }

    public class ServiceRegistration<TInterface, TService> : ServiceRegistration
        where TService : class, TInterface
    {
        public override Type ServiceType { get { return typeof(TService); } }
        
        public override Type InterfaceType { get { return typeof(TInterface); } }
    }
}