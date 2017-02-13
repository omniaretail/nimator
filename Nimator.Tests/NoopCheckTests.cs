using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NoopCheckTests
    {
        [Test]
        public void Constructor_WhenPassedNullArgument_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new NoopCheck(null));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        public void DefaultInstance_WhenGetName_ReturnsSensibleName(NotificationLevel level)
        {
            var sut = new NoopCheck(new NoopCheckSettings { LevelToSimulate = level });
            Assert.That(sut.ShortName, Is.Not.Null.And.Not.Empty);
            Assert.That(sut.ShortName, Does.Contain(level.ToString()));
        }
        
        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        public async Task DefaultInstance_WhenRun_ReturnsSuccess(NotificationLevel level)
        {
            var sut = new NoopCheck(new NoopCheckSettings { LevelToSimulate = level });
            var result = await sut.RunAsync();
            Assert.That(result.Level, Is.EqualTo(level));
        }

        [Test]
        public void InstanceWithException_WhenRun_ThrowsException()
        {
            var sut = new NoopCheck(new NoopCheckSettings { LevelToSimulate = NotificationLevel.Critical });
            Assert.ThrowsAsync<Exception>(async () => await sut.RunAsync());
        }
    }
}
