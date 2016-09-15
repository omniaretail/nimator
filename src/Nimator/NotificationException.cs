using System;
using System.Collections.Generic;

namespace Nimator
{
    [Serializable]
    internal class NotificationException : AggregateException
    {
        public NotificationException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        { }
    }
}
