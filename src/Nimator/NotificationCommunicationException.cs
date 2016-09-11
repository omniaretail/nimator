using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    [Serializable]
    public class NotificationCommunicationException : Exception
    {
        public NotificationCommunicationException() { }
        public NotificationCommunicationException(string message) : base(message) { }
        public NotificationCommunicationException(string message, Exception inner) : base(message, inner) { }
        protected NotificationCommunicationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
