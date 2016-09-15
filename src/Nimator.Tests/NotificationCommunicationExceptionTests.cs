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
