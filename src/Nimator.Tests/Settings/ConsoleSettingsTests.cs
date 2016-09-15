using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture] 
    public class ConsoleSettingsTests
    {
        [Test]
        public void Constructor_WhenCalled_SetsSensibleDefaults()
        {
            var sut = new ConsoleSettings();
            Assert.That(sut.Threshold, Is.EqualTo(NotificationLevel.Okay));
        }

        [Test]
        public void ToNotifier_ForDefaultSettings_ReturnsNotifier()
        {
            var sut = new ConsoleSettings();
            var result = sut.ToNotifier();
            Assert.That(result, Is.Not.Null);
        }
    }
}
