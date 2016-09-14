using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    [TestFixture]
    public class SlackNotifierTests : NotifierTests
    {
        private SlackSettings fakeSettings;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fakeSettings = new SlackSettings
            {
                DebounceTimeInSecs = 180,
                Threshold = NotificationLevel.Error,
                Url = "http://localhost/dummy/url",
            };
        }

        [Test]
        public void Constructor_WhenSettingsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new SlackNotifier(null));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsUrlInvalid_ThrowsException(string url)
        {
            var settings = new SlackSettings {Url = url};
            var exception = Assert.Throws<ArgumentException>(() => new SlackNotifier(settings));
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
            Assert.That(MostRecentRestUrlCalled, Is.EqualTo(fakeSettings.Url));
            Assert.That(MostRecentRestPayload, Is.InstanceOf<SlackMessage>());
        }

        [Test]
        public void Notify_WhenPassedCriticalResultTwice_WillRespectDebounce()
        {
            var sut = fakeSettings.ToNotifier();
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(fakeSettings.Threshold);
            sut.Notify(resultMock.Object);
            sut.Notify(resultMock.Object); // For sure this is before the debounce has passed
            Assert.That(NumberOfRestCalls, Is.EqualTo(1));
        }

        [Test]
        public void Notify_WhenPassedCriticalResult_WillPostSerializableObjectToRestApi()
        {
            var sut = fakeSettings.ToNotifier();
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(fakeSettings.Threshold);
            sut.Notify(resultMock.Object);
            var json = JsonConvert.SerializeObject(MostRecentRestPayload);
            Assert.That(json, Is.Not.Null.And.Not.Empty);
        }
    }
}
