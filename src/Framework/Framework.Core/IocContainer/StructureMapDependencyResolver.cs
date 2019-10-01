using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Common.Helper;
using Framework.Contracts.Helper;
using Framework.Contracts.IocContainer;
using Framework.Contracts.IocContainer.Registrations;
using Framework.Contracts.Services.DataAccess;
using StructureMap;

namespace Framework.Common.IocContainer
{
    public class StructureMapDependencyResolver : IDependencyResolver, IDependencyResolverConfigurator
    {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container ?? throw new ArgumentNullException("container");
        }


        public T GetInstance<T>() where T : class
        {
            var result = TryGetInstance<T>();
            if (result.HasValue)
            {
                return result.Value;
            }
            Console.WriteLine(_container.WhatDoIHave());
            throw new InvalidOperationException(string.Format("Can not resolve dependency '{0}'", typeof(T).FullName));
        }

        public object GetInstance(Type type)
        {
            var result = TryGetInstance(type);
            if (result.HasValue)
            {
                return result.Value;
            }
            throw new InvalidOperationException(string.Format("Can not resolve dependency '{0}'", type.FullName));
        }

        public IMaybe<object> TryGetInstance(Type type)
        {
            var result = _container.TryGetInstance(type);
            return new Maybe<object>(result);
        }

        public IMaybe<T> TryGetInstance<T>()
            where T : class
        {
            var result = _container.TryGetInstance<T>();
            return new Maybe<T>(result);
        }

        public IEnumerable<T> GetAllInstances<T>()
            where T : class
            => _container.GetAllInstances<T>().AsEnumerable();

        public void AddRegistration(DependencyResolverRegistration registration)
        {
            var visitor = new DependencyResolverConfigurationVisitor(_container);
            registration.Accept(visitor);
        }

        public IEnumerable<object> GetAllInstances(Type type)
            => _container.GetAllInstances(type).Cast<object>();

        public T CreateConcreteInstanceWithDependencies<T>()
            => _container.GetInstance<T>();

        private class DependencyResolverConfigurationVisitor : IDependencyResolverRegistrationVisitor
        {
            private readonly IContainer _container;

            public DependencyResolverConfigurationVisitor(IContainer container)
            {
                _container = container;
            }

            public void Handle(ManagerRegistration managerRegistration)
            {
                _container.Configure(x => x.For(managerRegistration.InterfaceType)
                                            .Singleton()
                                            .Use(managerRegistration.ConcreteManagerType));

                var manager = (IManager) _container.GetInstance(managerRegistration.InterfaceType);
                _container.Configure(x => x.For(typeof(IManager))
                                           .Add(manager));

                var managerType = typeof(IManager<>).MakeGenericType(new[] { manager.Description.TypeOfEntity });
                _container.Configure(x => x.For(managerType)
                                           .Singleton()
                                           .Use(manager));

                foreach (var type in manager.Description.DerivedEntities.Select(x => x.TypeOfEntity))
                {
                    _container.Configure(x => x.For(type)
                                           .Use(type));
                }

            }

            public void Handle(RepositoryRegistration repositoryRegistration)
                => _container.Configure(x => x.For(repositoryRegistration.InterfaceType)
                                              .Singleton()
                                              .Use(repositoryRegistration.RepositoryType));

            public void Handle(NamedServiceRegistration namedServiceRegistration)
                => _container.Configure(x => x.For(namedServiceRegistration.InterfaceType)
                                              .Use(namedServiceRegistration.ServiceType)
                                              .Named(namedServiceRegistration.Name));

            public void Handle(ServiceRegistration serviceRegistration)
                => _container.Configure(x => x.For(serviceRegistration.InterfaceType)
                                              .Use(serviceRegistration.ServiceType));

            public void Handle(SingletonRegistration serviceRegistration)
            {
                if (serviceRegistration.SingletonInstance == null)
                {
                    _container.Configure(x => x.For(serviceRegistration.InterfaceType)
                        .Singleton()
                        .Use(serviceRegistration.SingletonConcreteType));
                }
                else
                {
                    _container.Configure(x => x.For(serviceRegistration.InterfaceType)
                        .Singleton()
                        .Use(serviceRegistration.SingletonInstance));
                }
            }

            public void Handle(FactoryRegistration serviceRegistration)
                => _container.Configure(x =>
                   {
                       x.For(serviceRegistration.InterfaceType).Use("Factory registration", context =>
                       {
                           var factoryContext = new FactoryContext
                           {
                               ParentType = context.ParentType,
                               RequestedName = context.RequestedName
                           };
                           var factory = context.GetInstance(serviceRegistration.FactoryType);
                           return serviceRegistration.FactoryMethod(factory, factoryContext);
                       });
                   });

            public void Handle(ServiceInstanceRegistration serviceRegistration)
                => _container.Configure(x => x.For(serviceRegistration.InterfaceType)
                                              .Add(serviceRegistration.Instance));
        }
    }
}
