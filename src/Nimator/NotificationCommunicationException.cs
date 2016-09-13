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
        public NotificationCommunicationException(string message, Exception inner) 
            : base(message, inner)
        { }
    }
}
