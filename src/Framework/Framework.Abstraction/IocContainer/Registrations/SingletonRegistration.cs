using System;

namespace Framework.Contracts.IocContainer
{
    public abstract class SingletonRegistration : DependencyResolverRegistration
    {
        public object SingletonInstance { get; private set; }

        public Type SingletonConcreteType { get; private set; }

        protected SingletonRegistration(object singletonInstance)
        {
            SingletonInstance = singletonInstance ?? throw new ArgumentNullException("singletonInstance");
            SingletonConcreteType = singletonInstance.GetType();
        }

        protected SingletonRegistration(Type singletonType)
        {
            SingletonInstance = null;
            SingletonConcreteType = singletonType ?? throw new ArgumentNullException("singletonType");
        }

        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
            => visitor.Handle(this);
    }

    public class SingletonRegistration<TInterface> : SingletonRegistration
    {
        private readonly Type _singletonConcreteType;

        public override Type InterfaceType => typeof(TInterface);

        public SingletonRegistration(TInterface singletonInstance)
            : base(singletonInstance)
        {
            _singletonConcreteType = singletonInstance.GetType();
        }
    }

    public class SingletonRegistration<TInterface, TImplementation> : SingletonRegistration
        where TImplementation : TInterface
    {
        public override Type InterfaceType => typeof(TInterface);

        public SingletonRegistration() :
            base(typeof(TImplementation))
        { }
    }
}
