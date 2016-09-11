using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Nimator;

namespace Nimator
{
    [TestFixture]
    public class NoopCheckTests
    {
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

        public void InstanceWithException_WhenRun_ThrowsException()
        {
            var sut = new NoopCheck(new NoopCheckSettings { LevelToSimulate = NotificationLevel.Critical });
            Assert.ThrowsAsync<Exception>(async () => await sut.RunAsync());
        }
    }
}
