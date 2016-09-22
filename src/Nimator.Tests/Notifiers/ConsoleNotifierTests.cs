using System;
using Moq;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    public class ConsoleNotifierTests : NotifierTests
    {
        private ConsoleSettings fakeSettings;
        private Action<string> fakeWriteLine;
        private string lastWrittenLine;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fakeWriteLine = s =>
            {
                lastWrittenLine = s;
            };

            fakeSettings = new ConsoleSettings
            {
                Threshold = NotificationLevel.Warning
            };
        }

        [Test]
        public void Constructor_WhenSettingsIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsoleNotifier(null, fakeWriteLine));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [Test]
        public void Constructor_WhenWriteLineIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ConsoleNotifier(fakeSettings, null));
            Assert.That(exception.ParamName, Is.EqualTo("writeLine"));
        }

        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        [TestCase(Int32.MaxValue)] // Represents some yet unknown NotifcationLevel
        public void Notify_WhenPassedResultAtThreshold_WillCallWriteLine(NotificationLevel level)
        {
            fakeSettings.Threshold = level;
            var sut = new ConsoleNotifier(fakeSettings, fakeWriteLine);
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(fakeSettings.Threshold);
            resultMock.Setup(r => r.RenderPlainText(level)).Returns(level.ToString());

            sut.Notify(resultMock.Object);

            Assert.That(lastWrittenLine, Is.EqualTo(level.ToString()));
        }
    }
}
