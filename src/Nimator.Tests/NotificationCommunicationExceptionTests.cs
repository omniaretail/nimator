using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NotificationCommunicationExceptionTests
    {
        [Test]
        public void Constructor_WhenCalled_CreatesNewInstance()
        {
            Assert.DoesNotThrow(() => new NotificationCommunicationException("msg", null));
        }
    }
}
