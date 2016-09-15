using System;
using Moq;
using Newtonsoft.Json;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    [TestFixture]
    public class OpsGenieNotifierTests : NotifierTests
    {
        private OpsGenieSettings fakeSettings;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fakeSettings = new OpsGenieSettings
            {
                Threshold = NotificationLevel.Error,
                HeartbeatName = "dummy-heartbeat",
                TeamName = "dummy-team",
                ApiKey = "dummy-api-key",
            };
        }

        [Test]
        public void Constructor_WhenSettingsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieNotifier(null));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsApiKeyInvalid_ThrowsException(string input)
        {
            var settings = NewOpsGenieSettingsWith(s => s.ApiKey = input);
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsHeartbeatNameInvalid_ThrowsException(string input)
        {
            var settings = NewOpsGenieSettingsWith(s => s.HeartbeatName = input);
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsTeamNameInvalid_ThrowsException(string input)
        {
            var settings = NewOpsGenieSettingsWith(s => s.TeamName = input);
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [Test]
        public void Notify_SendsHeartbeat_RegardlessOfLevel()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns((NotificationLevel)Int32.MinValue);
            var sut = fakeSettings.ToNotifier();
            sut.Notify(resultMock.Object);
            Assert.That(MostRecentRestPayload, Is.InstanceOf<OpsGenieHeartbeatRequest>());
        }

        [Test]
        public void Notify_WithOnlyHeartbeat_PostsSerializableObjectToRestApi()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns((NotificationLevel)Int32.MinValue);
            var sut = fakeSettings.ToNotifier();
            sut.Notify(resultMock.Object);
            var json = JsonConvert.SerializeObject(MostRecentRestPayload);
            Assert.That(json, Is.Not.Null.And.Not.Empty);
        }

        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        [TestCase(Int32.MaxValue)] // Represents some yet unknown NotifcationLevel
        public void Notify_WhenPassedResultAtThreshold_PostsToRestApi(NotificationLevel level)
        {
            fakeSettings.Threshold = level;
            var sut = fakeSettings.ToNotifier();
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(fakeSettings.Threshold);
            sut.Notify(resultMock.Object);
            Assert.That(MostRecentRestPayload, Is.InstanceOf<OpsGenieCreateAlertRequest>());
        }

        [Test]
        public void Notify_WhenPassedCriticalResult_PostsSerializableObjectToRestApi()
        {
            var sut = fakeSettings.ToNotifier();
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(fakeSettings.Threshold);
            sut.Notify(resultMock.Object);
            var json = JsonConvert.SerializeObject(MostRecentRestPayload);
            Assert.That(json, Is.Not.Null.And.Not.Empty);
        }

        private OpsGenieSettings NewOpsGenieSettingsWith(Action<OpsGenieSettings> modify)
        {
            var settings = new OpsGenieSettings
            {
                ApiKey = "dummy-api-key",
                HeartbeatName = "dummy-hearbeat",
                TeamName = "dummy-teamname",
            };
            modify(settings);
            return settings;
        }
    }
}
