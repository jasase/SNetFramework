using System;
using Microsoft.Extensions.Logging;

namespace Framework.Abstraction.Extension
{
    public interface ILogManager : ILoggerFactory, ILoggerProvider
    {
        ILogger GetLogger(Type loggedClass);
        ILogger GetLogger<T>();
    }
}
