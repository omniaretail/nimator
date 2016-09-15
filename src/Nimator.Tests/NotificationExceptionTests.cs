using System;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NotificationExceptionTests
    {
        [Test]
        public void Constructor_WhenCalled_CanInstantiate()
        {
            Assert.DoesNotThrow(() => new NotificationException("msg", new Exception[0]));
        }
    }
}
