using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    [Serializable]
    public class NotificationException : AggregateException
    {
        public NotificationException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        { }
    }
}
