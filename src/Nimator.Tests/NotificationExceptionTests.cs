using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
