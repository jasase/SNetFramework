using Framework.Abstraction.Extension;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Extension.LoggingNLog
{
    public class LogManager : ILogManager
    {
        private LoggerFactory _loggerFactory;

        public LogManager()
        {
            _loggerFactory = new LoggerFactory();
            _loggerFactory.AddNLog();
        }

        public void AddProvider(ILoggerProvider provider)
            => _loggerFactory.AddProvider(provider);

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
                => _loggerFactory.CreateLogger(categoryName);

        public void Dispose()
                => _loggerFactory.Dispose();

        public Framework.Abstraction.Extension.ILogger GetLogger(Type loggedClass)
        {
            var nlogLogger = NLog.LogManager.GetLogger(loggedClass.FullName);
            return new Logger(nlogLogger);
        }

        public Framework.Abstraction.Extension.ILogger GetLogger<T>() => GetLogger(typeof(T));
    }
}
