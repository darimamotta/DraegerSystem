using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole.exceptions
{
    public class ReadConfigException : Exception
    {
        public ReadConfigException()
        {
        }

        public ReadConfigException(string? message) : base(message)
        {
        }

        public ReadConfigException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReadConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
