namespace Framework.Abstraction.IocContainer
{
    public interface IDependencyResolverConfigurator
    {
        void AddRegistration(DependencyResolverRegistration registration);
    }
}