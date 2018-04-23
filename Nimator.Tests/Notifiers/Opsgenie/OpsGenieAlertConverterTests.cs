using System;
using System.Linq;
using Moq;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers.Opsgenie
{
    [TestFixture]
    public class OpsGenieAlertConverterTests
    {
        private OpsGenieSettings settings;
        private OpsGenieAlertConverter sut;

        [SetUp]
        public void Setup()
        {
            settings = new OpsGenieSettings
            {
                Threshold = NotificationLevel.Error,
                HeartbeatName = "dummy-heartbeat",
                TeamName = "dummy-team",
                ApiKey = "dummy-api-key",
            };

            sut = new OpsGenieAlertConverter(settings);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsTeamNameInvalid_ThrowsException(string input)
        {
            settings.TeamName = input;

            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieAlertConverter(settings));

            Assert.That(exception.ParamName, Is.EqualTo("TeamName"));
        }

        [Test]
        public void Convert_WhenResultIsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => sut.Convert(null));

            Assert.That(exception.ParamName, Is.EqualTo("result"));
        }
        
        [TestCase(null, "Unknown message")]
        [TestCase("", "Unknown message")]
        [TestCase("some message", "some message")]
        public void Convert_WhenMessageIsFilled_ReturnsExpectedMessage(string input, string expected)
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Message).Returns(input);

            var result = sut.Convert(resultMock.Object);

            Assert.That(result.Message, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_WhenMessageIsTooLarge_ReturnsTruncatedMessage()
        {
            string input =
                "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            string expected = input.Substring(0, 130);

            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.Message).Returns(input);

            var result = sut.Convert(resultMock.Object);

            Assert.That(result.Message, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_WhenCalled_ReturnsExpectedAlias()
        {
            var resultMock = new Mock<INimatorResult>();

            var result = sut.Convert(resultMock.Object);

            Assert.That(result.Alias, Is.EqualTo("nimator-failure"));
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("some message", "some message")]
        public void Convert_WhenRenderPlainTextIsFilled_ReturnsExpectedMessage(string input, string expected)
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.RenderPlainText(settings.Threshold)).Returns(input);

            var result = sut.Convert(resultMock.Object);

            Assert.That(result.Description, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_WhenRenderPlainTextIsTooLarge_ReturnsTruncatedMessage()
        {
            string input = GenerateRandomString(15010);
            string expected = input.Substring(0, 15000);

            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.RenderPlainText(settings.Threshold)).Returns(input);

            var result = sut.Convert(resultMock.Object);

            Assert.That(result.Description, Is.EqualTo(expected));
        }

        [Test]
        public void Convert_WhenTeamNameIsSet_ReturnsExpectedResponders()
        {
            settings.TeamName = "Some team name";

            var resultMock = new Mock<INimatorResult>();
            var result = sut.Convert(resultMock.Object);

            var responder = result.Responders.Single();

            Assert.That(responder.Name, Is.EqualTo(settings.TeamName));
            Assert.That(responder.Type, Is.EqualTo(OpsGenieResponderType.Team));
        }

        [Test]
        public void Convert_WhenFirstFailedLayerName_ReturnsExpectedTags()
        {
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.GetFirstFailedLayerName()).Returns("Some layer");

            var result = sut.Convert(resultMock.Object);            

            Assert.That(result.Tags.Length, Is.EqualTo(2));
            Assert.That(result.Tags[0], Is.EqualTo("Nimator"));
            Assert.That(result.Tags[1], Is.EqualTo("Some layer"));
        }

        [Test]
        public void Convert_WhenFirstFailedLayerNameIsTooLong_ReturnsExpectedTags()
        {
            string input = GenerateRandomString(55);
            string expected = input.Substring(0, 50);
            
            var resultMock = new Mock<INimatorResult>();
            resultMock.Setup(r => r.GetFirstFailedLayerName()).Returns(input);

            var result = sut.Convert(resultMock.Object);
            
            Assert.That(result.Tags[1], Is.EqualTo(expected));
        }

        private static Random randomGenerator = new Random();
        internal static string GenerateRandomString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[randomGenerator.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
