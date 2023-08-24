using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole.Exceptions
{
    public class UnknownErrorException : Exception
    {
        public UnknownErrorException()
        {
        }

        public UnknownErrorException(string? message) : base(message)
        {
        }

        public UnknownErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnknownErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
