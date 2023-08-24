using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole.Exceptions
{
    public class ReadDBException : Exception
    {
        public ReadDBException()
        {
        }

        public ReadDBException(string? message) : base(message)
        {
        }

        public ReadDBException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReadDBException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
