using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole.exceptions
{
    public class InputFormatException : Exception
    {
        public InputFormatException()
        {
        }

        public InputFormatException(string? message) : base(message)
        {
        }

        public InputFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InputFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
