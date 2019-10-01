using Framework.Contracts.Entities;
using Framework.Contracts.IocContainer.Registrations;
using Framework.Contracts.Services.DataAccess;

namespace Framework.Contracts.IocContainer
{
    public interface IDependencyResolverRegistrationVisitor
    {
        void Handle(ServiceRegistration serviceRegistration);
        void Handle(ServiceInstanceRegistration serviceRegistration);
        void Handle(SingletonRegistration serviceRegistration);
        void Handle(RepositoryRegistration repositoryRegistration);
        void Handle(FactoryRegistration serviceRegistration);
        void Handle(ManagerRegistration managerRegistration);
        void Handle(NamedServiceRegistration namedServiceRegistration);
    }
}
