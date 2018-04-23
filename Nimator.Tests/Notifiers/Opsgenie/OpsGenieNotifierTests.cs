using System;
using Moq;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers.Opsgenie
{
    [TestFixture]
    public class OpsGenieNotifierTests
    {
        private Mock<IOpsGenieApiClient> client;
        private Mock<IOpsGenieAlertConverter> converter;
        private OpsGenieSettings settings;

        private OpsGenieNotifier sut;

        [SetUp]
        public void Setup()
        {
            client = new Mock<IOpsGenieApiClient>();
            converter = new Mock<IOpsGenieAlertConverter>();
            settings = new OpsGenieSettings
            {
                Threshold = NotificationLevel.Error,
                HeartbeatName = "dummy-heartbeat",
                TeamName = "dummy-team",
                ApiKey = "dummy-api-key",
            };

            sut = new OpsGenieNotifier(client.Object, converter.Object, settings);
        }

        [Test]
        public void Constructor_WhenApiClientIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieNotifier(null, converter.Object, settings));

            Assert.That(exception.ParamName, Is.EqualTo("client"));
        }

        [Test]
        public void Constructor_WhenConverterIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieNotifier(client.Object, null, settings));

            Assert.That(exception.ParamName, Is.EqualTo("converter"));
        }

        [Test]
        public void Constructor_WhenSettingsIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieNotifier(client.Object, converter.Object, null));

            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        
        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]

        public void Notify_WhenRegardlessOfLevel_VerifyHeartbeat(NotificationLevel level)
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(level);

            sut.Notify(resultMock.Object);

            client.Verify(_ => _.SendHeartbeat(), Times.Once);            
        }

        [Test]
        public void Notify_WhenNimatorResultLevelAboveThreshold_VerifyConvert()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(NotificationLevel.Critical);

            settings.Threshold = NotificationLevel.Error;

            sut.Notify(resultMock.Object);

            converter.Verify(_ => _.Convert(resultMock.Object), Times.Once);
        }

        [Test]
        public void Notify_WhenNimatorResultLevelAtThreshold_VerifyConvert()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(NotificationLevel.Critical);

            settings.Threshold = NotificationLevel.Critical;

            sut.Notify(resultMock.Object);

            converter.Verify(_ => _.Convert(resultMock.Object), Times.Once);
        }

        [Test]
        public void Notify_WhenNimatorResultLevelAboveThreshold_VerifySendAlert()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(NotificationLevel.Critical);

            var request = new OpsGenieAlertRequest("Some message");
            converter.Setup(_ => _.Convert(resultMock.Object)).Returns(request);

            settings.Threshold = NotificationLevel.Error;

            sut.Notify(resultMock.Object);

            client.Verify(_ => _.SendAlert(request), Times.Once);
        }

        [Test]
        public void Notify_WhenNimatorResultLevelAtThreshold_VerifySendAlert()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(NotificationLevel.Critical);

            settings.Threshold = NotificationLevel.Critical;

            var request = new OpsGenieAlertRequest("Some message");
            converter.Setup(_ => _.Convert(resultMock.Object)).Returns(request);

            sut.Notify(resultMock.Object);

            client.Verify(_ => _.SendAlert(request), Times.Once);
        }

        [Test]
        public void Notify_WhenNimatorResultLevelBelowThreshold_VerifyDonotSendAlert()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Level).Returns(NotificationLevel.Error);

            settings.Threshold = NotificationLevel.Critical;

            var request = new OpsGenieAlertRequest("Some message");
            converter.Setup(_ => _.Convert(resultMock.Object)).Returns(request);

            sut.Notify(resultMock.Object);

            client.Verify(_ => _.SendAlert(request), Times.Never);
        }        
    }
}
