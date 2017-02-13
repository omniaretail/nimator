using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NoopCheckSettingsTests
    {
        [Test]
        public void ToCheck_ForDefaultInstance_ReturnsNoopCheck()
        {
            var sut = new NoopCheckSettings();
            var result = sut.ToCheck();
            Assert.That(result, Is.InstanceOf<NoopCheck>());
        }
    }
}
