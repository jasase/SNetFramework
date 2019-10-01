using System;
using System.Runtime.Serialization;

namespace Framework.Core.Exceptions
{
    [Serializable]
    public class InvalidServiceConfigurationException : Exception
    {
        private readonly string _ServiceName;

        public string ServiceName
        {
            get { return _ServiceName; }
        }

        public InvalidServiceConfigurationException()
        { }

        public InvalidServiceConfigurationException(string message)
            : base(message)
        { }

        public InvalidServiceConfigurationException(string serviceName, string message)
            : base(message)
        {
            _ServiceName = serviceName;
        }

        public InvalidServiceConfigurationException(string message, Exception inner)
            : base(message, inner)
        { }

        protected InvalidServiceConfigurationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }
}