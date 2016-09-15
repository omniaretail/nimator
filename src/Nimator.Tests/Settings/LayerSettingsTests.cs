using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class LayerSettingsTests
    {
        [Test]
        public void Constructor_WhenCalled_SetsSensibleDefaults()
        {
            var sut = new LayerSettings();
            Assert.That(sut.Checks, Is.Not.Null.And.Empty);
        }
    }
}
