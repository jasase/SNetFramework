using System;

namespace Framework.Abstraction.IocContainer.Registrations
{
    public class ServiceInstanceRegistration : DependencyResolverRegistration
    {
        public override Type InterfaceType { get; }
        public object Instance { get; }

        protected ServiceInstanceRegistration(Type interfaceType, object instance)
        {
            InterfaceType = interfaceType ?? throw new ArgumentNullException(nameof(interfaceType));
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));

            if (!InterfaceType.IsAssignableFrom(Instance.GetType()))
            {
                throw new ArgumentException("InterfaceType and type of instance does not matching");
            }
        }

        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
            => visitor.Handle(this);
    }

    public class ServiceInstanceRegistration<TInterface> : ServiceInstanceRegistration
    {
        public ServiceInstanceRegistration(TInterface instance)
            : base(typeof(TInterface), instance)
        { }
    }
}
