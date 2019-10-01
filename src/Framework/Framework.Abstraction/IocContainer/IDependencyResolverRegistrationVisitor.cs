using Framework.Abstraction.Entities;
using Framework.Abstraction.IocContainer.Registrations;
using Framework.Abstraction.Services.DataAccess;

namespace Framework.Abstraction.IocContainer
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
