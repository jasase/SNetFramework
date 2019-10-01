using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess;
using System;

namespace Framework.Contracts.IocContainer.Registrations
{
    public abstract class ManagerRegistration : DependencyResolverRegistration
    {
        public abstract Type ConcreteManagerType { get; }

        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
        {
            visitor.Handle(this);
        }
    }

    public class ManagerRegistration<TManagerInterface, TManager, TEntity> : ManagerRegistration
        where TEntity : Entity
        where TManagerInterface : IManager<TEntity>
        where TManager : TManagerInterface
    {
        public override Type InterfaceType { get { return typeof(TManagerInterface); } }
        public override Type ConcreteManagerType { get { return typeof(TManager); } }

       
    }
}
