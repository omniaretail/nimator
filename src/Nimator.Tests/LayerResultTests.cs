using System;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class LayerResultTests
    {
        [Test]
        public void Constructor_WhenGivenNoName_ThrowsException()
        {
            var checkResults = new CheckResult[0];

            Assert.Throws<ArgumentException>(() => new LayerResult(null, checkResults));
            Assert.Throws<ArgumentException>(() => new LayerResult("", checkResults));
            Assert.Throws<ArgumentException>(() => new LayerResult("       ", checkResults));
        }

        [Test]
        public void Constructor_WhenGivenNullForResults_CoalescesToZeroResultsAndWarning()
        {
            var sut = new LayerResult("Layer A", new CheckResult[0]);
            Assert.That(sut.CheckResults, Is.Empty);
            Assert.That(sut.Level, Is.EqualTo(NotificationLevel.Warning), "Expected LayerResult to be a Warning if there was zero child CheckResults.");
        }

        [Test]
        public void Constructor_WhenGivenMultipleCheckResults_AssumesWorstNotificationLevel()
        {
            var checkresults = new[]
            {
                new CheckResult("check 1", NotificationLevel.Okay),
                new CheckResult("check 2", NotificationLevel.Error), // Dead in the middle is the "worst" result
                new CheckResult("check 3", NotificationLevel.Okay),
            };

            var sut = new LayerResult("layer A", checkresults);

            Assert.That(sut.Level, Is.EqualTo(NotificationLevel.Error));
        }

        [Test]
        public void Constructor_WhenPassedNullInsideSubResults_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new LayerResult("layer1", new CheckResult[] { null }));
        }

        [Test]
        public void ToString_WhenResultIsMeaningFul_CombinesAllParts()
        {
            var checkresults = new[]
            {
                new CheckResult("check i", NotificationLevel.Okay),
                new CheckResult("check ii", NotificationLevel.Error),
            };

            var sut = new LayerResult("layer A", checkresults);

            var result = sut.RenderPlainText();

            Assert.That(result, Does.Contain("layer A"));
            Assert.That(result, Does.Contain("Error"));
            Assert.That(result, Does.Contain("2"), "Expected number of child CheckResults to be in ToString value.");
        }

        [Test]
        public void ToString_WhenChecksHaveErrors_IncludesCheckNames()
        {
            var checkresults = new[]
            {
                new CheckResult("check a", NotificationLevel.Okay),
                new CheckResult("check b", NotificationLevel.Warning),
                new CheckResult("check c", NotificationLevel.Error),
                new CheckResult("check d", NotificationLevel.Error),
                new CheckResult("check e", NotificationLevel.Critical),
            };

            var sut = new LayerResult("layer A", checkresults);

            var result = sut.RenderPlainText();

            Assert.That(result.ToLowerInvariant(), Does.Not.Contain("check a"));
            Assert.That(result.ToLowerInvariant(), Does.Not.Contain("check b"));
            Assert.That(result.ToLowerInvariant(), Does.Contain("check c"));
            Assert.That(result.ToLowerInvariant(), Does.Contain("check d"));
            Assert.That(result.ToLowerInvariant(), Does.Contain("check e"));
        }

        [Test]
        public void ToString_WhenChecksHaveErrors_IncludesMessages()
        {
            var checkresults = new[]
            {
                new CheckResult("check-a", NotificationLevel.Error, "custom message abc"),
                new CheckResult("check-b", NotificationLevel.Critical, "custom message xyz"),
            };

            var sut = new LayerResult("layer A", checkresults);

            var result = sut.RenderPlainText();

            Assert.That(result.ToLowerInvariant(), Does.Contain("abc"));
            Assert.That(result.ToLowerInvariant(), Does.Contain("xyz"));
        }
    }
}
