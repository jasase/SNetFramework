using Framework.Contracts.Extension;
using LoggingExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.LoggingNLog
{
    public class Logger : ILogger
    {
        private readonly NLog.ILogger _logger;

        public Logger(NLog.ILogger logger)
        {
            _logger = logger;
        }

        public void Debug(string message) => Log(NLog.LogLevel.Debug, message);
        public void Debug(string format, params object[] message) => Log(NLog.LogLevel.Debug, format, message);
        public void Debug(string format, params Func<string>[] messages) => LogLazy(NLog.LogLevel.Debug, format, messages);
        public void DebugDump(string message, object dump)
        {
            Func<string> dumpMessage = () =>
            {
                var result = new StringBuilder();
                result.AppendLine(message);
                var writer = new StringWriter(result);
                ObjectDumper.Write(dump, 3, writer);
                return result.ToString();
            };
            Debug("{0}", dumpMessage);
        }
        public void DebugDump(string format, object dump, params Func<string>[] messages)
        {
            Func<string> dumpMessage = () =>
            {
                var result = new StringBuilder();
                result.AppendLine(String.Format(format, messages.Select(x => x()).ToArray()));
                var writer = new StringWriter(result);
                ObjectDumper.Write(dump, 3, writer);
                return result.ToString();
            };
            Debug("{0}", dumpMessage);
        }

        public void Error(string message) => Log(NLog.LogLevel.Error, message);
        public void Error(string format, params object[] message) => Log(NLog.LogLevel.Error, format, message);
        public void Error(string format, params Func<string>[] messages) => LogLazy(NLog.LogLevel.Error, format, messages);
        public void Error(Exception exception, string message) => Log(NLog.LogLevel.Error, exception, message);
        public void Error(Exception exception, string format, params object[] message) => Log(NLog.LogLevel.Error, exception, format, message);
        public void Error(Exception exception, string format, params Func<string>[] messages) => LogLazy(NLog.LogLevel.Error, exception, format, messages);


        public void Info(string message) => Log(NLog.LogLevel.Info, message);
        public void Info(string format, params object[] message) => Log(NLog.LogLevel.Info, format, message);
        public void Info(string format, params Func<string>[] messages) => LogLazy(NLog.LogLevel.Info, format, messages);


        public void Warn(string message) => Log(NLog.LogLevel.Warn, message);
        public void Warn(string format, params object[] message) => Log(NLog.LogLevel.Warn, format, message);
        public void Warn(string format, params Func<string>[] messages) => LogLazy(NLog.LogLevel.Warn, format, messages);

        private void Log(NLog.LogLevel level, string format, params object[] msg)
        {
            _logger.Log(level, format, msg);
        }
        private void Log(NLog.LogLevel level, Exception ex, string format, params object[] msg)
        {
            _logger.Log(level, ex, format, msg);
        }
        private void LogLazy(NLog.LogLevel level, string format, Func<string>[] msg)
        {
            if (_logger.IsEnabled(level))
            {
                Log(level, format, msg.Select(x => x()).ToArray());
            }            
        }

        private void LogLazy(NLog.LogLevel level, Exception ex, string format, Func<string>[] msg)
        {
            if (_logger.IsEnabled(level))
            {
                Log(level, ex, format, msg.Select(x => x()).ToArray());
            }
        }
    }
}
