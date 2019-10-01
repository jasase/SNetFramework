using System;
using System.Runtime.Serialization;

namespace Framework.Common.Exceptions
{
    [Serializable]
    public class DataValidationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public DataValidationException()
        {
        }

        public DataValidationException(string message) : base(message)
        {
        }

        public DataValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DataValidationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
