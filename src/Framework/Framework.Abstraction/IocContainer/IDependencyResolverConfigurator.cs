namespace Framework.Contracts.IocContainer
{
    public interface IDependencyResolverConfigurator
    {
        void AddRegistration(DependencyResolverRegistration registration);
    }
}