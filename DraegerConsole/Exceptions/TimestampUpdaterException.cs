using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole.Exceptions
{
    public class TimestampUpdaterException : Exception
    {
        public TimestampUpdaterException()
        {
        }

        public TimestampUpdaterException(string? message) : base(message)
        {
        }

        public TimestampUpdaterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TimestampUpdaterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
