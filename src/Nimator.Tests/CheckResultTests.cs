using System;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class CheckResultTests
    {
        [Test]
        public void Constructor_WhenCheckNameIsInvalid_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new CheckResult(null, NotificationLevel.Okay));
            Assert.Throws<ArgumentException>(() => new CheckResult("", NotificationLevel.Okay));
            Assert.Throws<ArgumentException>(() => new CheckResult("     ", NotificationLevel.Okay));
        }

        [Test]
        public void ToString_WhenCalled_IncludesNameAndLevelAndMessage()
        {
            var sut = new CheckResult("Cas Node Check",  NotificationLevel.Warning, "custom message");
            var result = sut.ToString();
            Assert.That(result, Does.Contain("Cas Node Check"));
            Assert.That(result, Does.Contain("Warning"));
            Assert.That(result, Does.Contain("custom message"));
        }
    }
}
