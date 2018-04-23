using System;
using NUnit.Framework;

namespace Nimator.Notifiers.Opsgenie
{
    [TestFixture]
    public class OpsGenieAlertRequestTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void Constructor_WhenMessageIsInvalid_ThrowsException(string message)
        {
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieAlertRequest(message));

            Assert.That(exception.ParamName, Is.EqualTo("message"));
        }
    }
}
