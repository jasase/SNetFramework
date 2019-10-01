using Framework.Contracts.Entities;
using Framework.Contracts.Services.DataAccess;
using System;

namespace Framework.Contracts.IocContainer.Registrations
{
    public abstract class RepositoryRegistration : DependencyResolverRegistration
    {
        public abstract Type RepositoryType { get; }


        public override void Accept(IDependencyResolverRegistrationVisitor visitor)
        {
            visitor.Handle(this);
        }
    }

    public class RepositoryRegistration<TRepositoryInterface, TRepository, TEntity> : RepositoryRegistration
        where TEntity : Entity
        where TRepositoryInterface : IRepository<TEntity>
        where TRepository : TRepositoryInterface
    {
        public override Type InterfaceType { get { return typeof(TRepositoryInterface); } }
        public override Type RepositoryType { get { return typeof(TRepository); } }
    }
}
