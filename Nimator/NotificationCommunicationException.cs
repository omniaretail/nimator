using System;

namespace Nimator
{
    [Serializable]
    internal class NotificationCommunicationException : Exception
    {
        public NotificationCommunicationException(string message, Exception inner) 
            : base(message, inner)
        { }
    }
}
