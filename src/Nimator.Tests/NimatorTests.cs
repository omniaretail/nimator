using log4net;
using Moq;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NimatorTests
    {
        private Mock<ILog> loggerMock;

        [SetUp]
        public void SetUp()
        {
            loggerMock = new Mock<ILog>();
        }

        [Test]
        public void FromSettings_ForMostBasicJson_ReturnsNimator()
        {
            // Bit of a smoke test, but better than nothing...
            var json = "{}";
            var result = Nimator.FromSettings(loggerMock.Object, json);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void FromSettings_WhenGivenExampleAsJson_ReturnsNimator()
        {
            // Bit of a smoke test, but better than nothing...
            var example = NimatorSettings.GetExample();
            var json = example.ToJson();
            var result = Nimator.FromSettings(loggerMock.Object, json);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void FromSettings_WhenNoNotifiersConfigured_LogsWarning()
        {
            var example = NimatorSettings.GetExample();
            example.Notifiers = new NotifierSettings[0];
            var result = Nimator.FromSettings(loggerMock.Object, example.ToJson());
            loggerMock.Verify(l => l.Warn(It.Is<string>(s => s != null && s.ToLowerInvariant().Contains("notifiers"))));
        }
    }
}
