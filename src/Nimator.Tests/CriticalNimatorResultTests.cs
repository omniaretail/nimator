using System;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class CriticalNimatorResultTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenGivenNullMessage_Throws(string message)
        {
            var exc = Assert.Throws<ArgumentException>(() => new CriticalNimatorResult(message, ""));
            Assert.That(exc.ParamName, Is.EqualTo("message"));
            Assert.That(exc.Message, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Constructor_WhenCalled_SetsSensibleDefaults()
        {
            var sut = new CriticalNimatorResult("dummy message", "longer full text here");
            Assert.That(sut.Started, Is.Not.EqualTo(default(DateTime)));
            Assert.That(sut.Finished, Is.Not.EqualTo(default(DateTime)));
            Assert.That(sut.Message, Is.EqualTo("dummy message"));
            CollectionAssert.IsEmpty(sut.LayerResults);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenNotGivenFullText_RendersWithMessage(string fullText)
        {
            var sut = new CriticalNimatorResult("dummy message", fullText);
            Assert.That(sut.RenderPlainText(), Is.EqualTo("dummy message"));
        }

        [Test]
        public void RenderPlainText_WhenCalled_ReturnsFullText()
        {
            var sut = new CriticalNimatorResult("dummy message", "longer full text here");
            Assert.That(sut.RenderPlainText(), Is.EqualTo("longer full text here"));
        }

        [TestCase(NotificationLevel.Okay)]
        [TestCase(NotificationLevel.Warning)]
        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        public void RenderPlainText_WhenProvidedAnyThreshold_ReturnsFullText(NotificationLevel threshold)
        {
            var sut = new CriticalNimatorResult("dummy message", "longer full text here");
            Assert.That(sut.RenderPlainText(threshold), Is.EqualTo("longer full text here"));
        }

        [Test]
        public void GetFirstFailedLayerName_ForDefaultInstance_ReturnsConstText()
        {
            var sut = new CriticalNimatorResult("msg", "full txt");
            Assert.That(sut.GetFirstFailedLayerName(), Is.EqualTo("UnknownLayer"));
        }

        [Test]
        public void GetFailingLayerNames_ForDefaultInstance_ReturnsConstText()
        {
            var sut = new CriticalNimatorResult("msg", "full txt");
            CollectionAssert.AreEqual(sut.GetFailingLayerNames(), new[] { "UnknownLayer" });
        }

        [Test]
        public void GetFailingCheckNames_ForDefaultInstance_ReturnsConstText()
        {
            var sut = new CriticalNimatorResult("msg", "full txt");
            CollectionAssert.AreEqual(sut.GetFailingCheckNames(), new[] { "UnknownCheck" });
        }
    }
}
