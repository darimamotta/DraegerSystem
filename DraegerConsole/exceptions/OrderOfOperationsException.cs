using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraegerConsole.Exceptions
{
    public class OrderOfOperationsException : Exception
    {
        public OrderOfOperationsException()
        {
        }

        public OrderOfOperationsException(string? message) : base(message)
        {
        }

        public OrderOfOperationsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderOfOperationsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
