using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos
{
    [Serializable()]
    public class ConnectionException : Exception
    {
        public ConnectionException()
            : base()
        {
        }

        protected ConnectionException(System.Runtime.Serialization.SerializationInfo info,
                                    System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }

        public ConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
        public ConnectionException(string message)
            : base(message)
        {

        }
    }
}
