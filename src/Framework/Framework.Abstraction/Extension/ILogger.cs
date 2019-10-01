using System;

namespace Framework.Abstraction.Extension
{
    public interface ILogger
    {
        void Debug(string message);
        void DebugDump(string message, object dump);
        void DebugDump(string format, object dump, params Func<string>[] messages);
        void Debug(string format, params object[] message);
        void Debug(string format, params Func<string>[] messages);

        void Info(string message);        
        void Info(string format, params object[] message);
        void Info(string format, params Func<string>[] messages);

        void Warn(string message);        
        void Warn(string format, params object[] message);
        void Warn(string format, params Func<string>[] messages);

        void Error(string message);
        void Error(string format, params object[] message);
        void Error(string format, params Func<string>[] messages);
        void Error(Exception exception, string message);
        void Error(Exception exception, string format, params object[] message);
        void Error(Exception exception, string format, params Func<string>[] messages);
    }
}
