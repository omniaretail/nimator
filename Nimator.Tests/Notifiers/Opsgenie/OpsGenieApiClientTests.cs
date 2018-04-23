using System;
using System.Net;
using Moq;
using Newtonsoft.Json;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers.Opsgenie
{
    [TestFixture]
    public class OpsGenieApiClientTests
    {
        private Mock<IHttpRequestHandler> httpHandler;
        private OpsGenieSettings settings;

        private OpsGenieApiClient sut;

        [SetUp]
        public void Setup()
        {
            httpHandler = new Mock<IHttpRequestHandler>();

            settings = new OpsGenieSettings
            {
                Threshold = NotificationLevel.Error,
                HeartbeatName = "dummy-heartbeat",
                TeamName = "dummy-team",
                ApiKey = "dummy-api-key",
            };

            sut = new OpsGenieApiClient(httpHandler.Object, settings);
        }

        [Test]
        public void Constructor_WhenHttpHandlerIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieApiClient(null, settings));

            Assert.That(exception.ParamName, Is.EqualTo("httpHandler"));
        }

        [Test]
        public void Constructor_WhenSettingsIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieApiClient(httpHandler.Object, null));

            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsApiKeyInvalid_ThrowsException(string input)
        {
            settings.ApiKey = input;

            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieApiClient(httpHandler.Object, settings));

            Assert.That(exception.ParamName, Is.EqualTo("ApiKey"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsHeartbeatNameInvalid_ThrowsException(string input)
        {
            settings.HeartbeatName = input;

            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieApiClient(httpHandler.Object, settings));

            Assert.That(exception.ParamName, Is.EqualTo("HeartbeatName"));
        }

        [Test]
        public void SendHeartbeat_WhenHeartbeatNameIsSet_VerifyCreate()
        {
            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));                
                            
            sut.SendHeartbeat();

            httpHandler.Verify(_ => _.CreateRequest($"https://api.opsgenie.com/v2/heartbeats/{settings.HeartbeatName}/ping"), Times.Once);
        }

        [Test]
        public void SendHeartbeat_WhenHeartbeatNameContainsIlligalCharacters_VerifyUrlEncodedCreate()
        {
            settings.HeartbeatName = "dummy&heartbeat%withillegal#chars";
            sut = new OpsGenieApiClient(httpHandler.Object, settings);

            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            sut.SendHeartbeat();

            httpHandler.Verify(_ => _.CreateRequest("https://api.opsgenie.com/v2/heartbeats/dummy%26heartbeat%25withillegal%23chars/ping"), Times.Once);
        }

        [Test]
        public void SendHeartbeat_WhenInvoked_VerifyHandleRequest()
        {
            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            sut.SendHeartbeat();

            httpHandler.Verify(_ => _.HandleRequest(It.IsAny<HttpWebRequest>()), Times.Once);
        }

        [Test]
        public void SendHeartbeat_WhenInvoked_VerifyRequest()
        {
            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            HttpWebRequest handledRequest = null;

            httpHandler
                .Setup(_ => _.HandleRequest(It.IsAny<HttpWebRequest>()))
                .Callback((HttpWebRequest r) => { handledRequest = r; });

            sut.SendHeartbeat();

            Assert.That(handledRequest, Is.Not.Null);
            Assert.That(handledRequest.Method, Is.EqualTo("GET"));
            Assert.That(handledRequest.Headers["Authorization"], Is.EqualTo($"GenieKey { settings.ApiKey}"));            
        }

        [Test]
        public void SendAlert_WhenInvoked_VerifyCreate()
        {
            var alert = new OpsGenieAlertRequest("some message");

            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            sut.SendAlert(alert);

            httpHandler.Verify(_ => _.CreateRequest("https://api.opsgenie.com/v2/alerts"), Times.Once);
        }

        [Test]
        public void SendAlert_WhenInvoked_VerifyHandleRequest()
        {
            var alert = new OpsGenieAlertRequest("some message");

            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            sut.SendAlert(alert);

            httpHandler.Verify(_ => _.HandleRequest(It.IsAny<HttpWebRequest>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SendAlert_WhenInvoked_VerifyRequest()
        {
            var alert = new OpsGenieAlertRequest("some message");

            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            HttpWebRequest handledRequest = null;

            httpHandler
                .Setup(_ => _.HandleRequest(It.IsAny<HttpWebRequest>(), It.IsAny<string>()))
                .Callback((HttpWebRequest r, string c) => { handledRequest = r; });

            sut.SendAlert(alert);

            Assert.That(handledRequest, Is.Not.Null);

            Assert.That(handledRequest.Method, Is.EqualTo("POST"));
            Assert.That(handledRequest.Headers["Authorization"], Is.EqualTo($"GenieKey { settings.ApiKey}"));
            Assert.That(handledRequest.Accept, Is.EqualTo("application/json"));
            Assert.That(handledRequest.ContentType, Is.EqualTo("application/json"));
            Assert.That(handledRequest.KeepAlive, Is.EqualTo(false));
        }

        [Test]
        public void SendAlert_WhenInvoked_VerifyRequestContent()
        {
            var alert = new OpsGenieAlertRequest("Test alert")
            {                
                Alias = "nimator-failure",
                Description = "This is the rendered text from the check",
                Responders = new[]
                {
                    new OpsGenieResponder
                    {
                        Type = OpsGenieResponderType.Team,
                        Name = "Some opsgenie team"
                    }
                },
                Tags = new[] { "Nimator", "Some failed layer" }
            };

            httpHandler
                .Setup(_ => _.CreateRequest(It.IsAny<string>()))
                .Returns((string url) => (HttpWebRequest)WebRequest.Create(url));

            string handledContent = null;

            httpHandler
                .Setup(_ => _.HandleRequest(It.IsAny<HttpWebRequest>(), It.IsAny<string>()))
                .Callback((HttpWebRequest r, string content) => { handledContent = content; });

            sut.SendAlert(alert);

            var expectedContent = JsonConvert.SerializeObject(alert);

            Assert.That(handledContent, Is.EqualTo(expectedContent));
        }        
    }
}
