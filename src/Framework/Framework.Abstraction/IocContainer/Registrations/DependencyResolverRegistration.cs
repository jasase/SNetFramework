using System;

namespace Framework.Abstraction.IocContainer
{
    public abstract class DependencyResolverRegistration
    {
        public abstract Type InterfaceType { get; }
 
        public abstract void Accept(IDependencyResolverRegistrationVisitor visitor);
    }
}