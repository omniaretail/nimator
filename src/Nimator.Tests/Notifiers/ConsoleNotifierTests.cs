using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    public class ConsoleNotifierTests : NotifierTests
    {
        private ConsoleSettings fakeSettings;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fakeSettings = new ConsoleSettings
            {
                Threshold = NotificationLevel.Warning
            };
        }

        [Test]
        public void Constructor_WhenSettingsIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsoleNotifier(null));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        [TestCase(Int32.MaxValue)] // Represents some yet unknown NotifcationLevel
        public void Notify_WhenPassedResultAtThreshold_WillPostToRestApi(NotificationLevel level)
        {
            fakeSettings.Threshold = level;
            var sut = fakeSettings.ToNotifier();
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(fakeSettings.Threshold);
            sut.Notify(resultMock.Object);

            // Cannot easily assert Console.WriteLine calls without adding
            // enterprise-level overkill to factor out the Console depenency
            // so we'll just make this a smoke test: if we get here without
            // exception, everything should be fine.
        }
    }
}
