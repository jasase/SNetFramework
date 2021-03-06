﻿using System.Collections.Generic;
using Framework.Abstraction.Helper;
using System;

namespace Framework.Abstraction.IocContainer
{
    public interface IDependencyResolver
    {        
        object GetInstance(Type type);

        IMaybe<object> TryGetInstance(Type type);

        T GetInstance<T>()
            where T : class;

        IMaybe<T> TryGetInstance<T>()
            where T : class;

        IEnumerable<T> GetAllInstances<T>()
            where T : class;

        IEnumerable<object> GetAllInstances(Type type);

        T CreateConcreteInstanceWithDependencies<T>();
    }
}
