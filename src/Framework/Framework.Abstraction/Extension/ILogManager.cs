using System;
using Microsoft.Extensions.Logging;

namespace Framework.Contracts.Extension
{
    public interface ILogManager : ILoggerFactory, ILoggerProvider
    {
        ILogger GetLogger(Type loggedClass);
        ILogger GetLogger<T>();
    }
}
