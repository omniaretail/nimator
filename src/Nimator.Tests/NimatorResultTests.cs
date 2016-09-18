using System;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NimatorResultTests
    {
        [Test]
        public void Level_WhenNoResultsAvailable_ReturnsWarning()
        {
            var sut = new NimatorResult(DateTime.Now);
            sut.LayerResults.Clear();
            Assert.That(sut.Level, Is.EqualTo(NotificationLevel.Warning));
        }

        [Test]
        public void Level_WhenLayersHaveDifferingResults_ChoosesWorstLevel()
        {
            var sut = new NimatorResult(DateTime.Now);

            sut.LayerResults.Add(TestLayerResult("layer 1", NotificationLevel.Okay));
            sut.LayerResults.Add(TestLayerResult("layer 2", NotificationLevel.Warning));

            Assert.That(sut.Level, Is.EqualTo(NotificationLevel.Warning));
        }

        [Test]
        public void GetFirstFailedLayerName_WhenNoFailuresAvailable_ReturnsEmptyString()
        {
            var sut = new NimatorResult(DateTime.Now);
            var result = sut.GetFirstFailedLayerName();
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void GetFirstFailedLayerName_WhenOneFailuresAvailable_ReturnsName()
        {
            var sut = new NimatorResult(DateTime.Now);
            sut.LayerResults.Add(TestLayerResult("A", NotificationLevel.Error));
            var result = sut.GetFirstFailedLayerName();
            Assert.That(result, Is.EqualTo("A"));
        }

        [Test]
        public void GetFirstFailedLayerName_WhenManyFailuresAvailable_ReturnsFirstName()
        {
            var sut = new NimatorResult(DateTime.Now);
            sut.LayerResults.Add(TestLayerResult("A", NotificationLevel.Okay));
            sut.LayerResults.Add(TestLayerResult("B", NotificationLevel.Error));
            sut.LayerResults.Add(TestLayerResult("C", NotificationLevel.Error));
            var result = sut.GetFirstFailedLayerName();
            Assert.That(result, Is.EqualTo("B"));
        }

        [Test]
        public void GetFailingLayerNames_WhenNoResultsAvailable_ReturnsEmptyArray()
        {
            var sut = new NimatorResult(DateTime.Now);
            var result = sut.GetFailingLayerNames();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetFailingLayerNames_WhenErrorsAvailable_ReturnsLayerNames()
        {
            var sut = new NimatorResult(DateTime.Now);
            sut.LayerResults.Add(TestLayerResult("err", NotificationLevel.Error));
            sut.LayerResults.Add(TestLayerResult("crit", NotificationLevel.Critical));
            sut.LayerResults.Add(TestLayerResult("warn", NotificationLevel.Warning));
            sut.LayerResults.Add(TestLayerResult("ok", NotificationLevel.Okay));
            var result = sut.GetFailingLayerNames();
            CollectionAssert.AreEquivalent(new[] { "err", "crit" }, result);
        }

        [Test]
        public void GetFailingCheckNames_WhenNoResults_ReturnsEmptyArray()
        {
            var sut = new NimatorResult(DateTime.Now);
            var result = sut.GetFailingCheckNames();
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetFailingCheckNames_WhenErrorsAvailable_ReturnsNames()
        {
            var sut = new NimatorResult(DateTime.Now);

            sut.LayerResults.Add(new LayerResult("A", new[] {
                new CheckResult("c1", NotificationLevel.Okay),
                new CheckResult("c2", NotificationLevel.Warning),
                new CheckResult("c3", NotificationLevel.Error),
                new CheckResult("c4", NotificationLevel.Critical),
            }));

            sut.LayerResults.Add(new LayerResult("B", new[] {
                new CheckResult("c5", NotificationLevel.Okay),
                new CheckResult("c6", NotificationLevel.Warning),
                new CheckResult("c7", NotificationLevel.Error),
                new CheckResult("c8", NotificationLevel.Critical),
            }));

            var result = sut.GetFailingCheckNames();

            CollectionAssert.AreEquivalent(new[] { "c3", "c4", "c7", "c8" }, result);
        }

        [Test]
        public void Message_OnOkay_ReturnsSensibleText()
        {
            var sut = new NimatorResult(DateTime.Now) { Level = NotificationLevel.Okay };
            var result = sut.Message;
            Assert.That(result.ToLowerInvariant(), Does.Contain("okay"));
        }

        [Test]
        public void Message_OnStatusUnknown_ReturnsSensibleText()
        {
            var sut = new NimatorResult(DateTime.Now) { Level = (NotificationLevel)Int32.MaxValue };
            var result = sut.Message;
            Assert.That(result.ToLowerInvariant(), Does.Contain("unknown"));
        }

        [Test]
        public void Message_OnWarnings_ReturnsSensibleText()
        {
            var sut = new NimatorResult(DateTime.Now) { Level = NotificationLevel.Warning };
            var result = sut.Message;
            Assert.That(result.ToLowerInvariant(), Does.Contain("warn"));
        }

        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        public void Message_OnFailures_ReturnsSensibleText(NotificationLevel level)
        {
            var sut = new NimatorResult(DateTime.Now) { Level = level };
            var result = sut.Message;
            Assert.That(result.ToLowerInvariant(), Does.Contain("fail"));
        }

        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        public void Message_OnFailures_ReturnsTextWithFailingLayerNames(NotificationLevel level)
        {
            var sut = new NimatorResult(DateTime.Now) { Level = level };
            sut.LayerResults.Add(TestLayerResult("layer-1", level));
            sut.LayerResults.Add(TestLayerResult("layer-2", level));
            var result = sut.Message;
            Assert.That(result.ToLowerInvariant(), Does.Contain("layer-1"));
            Assert.That(result.ToLowerInvariant(), Does.Contain("layer-2"));
        }

        [TestCase(NotificationLevel.Error)]
        [TestCase(NotificationLevel.Critical)]
        public void Message_OnFailures_ReturnsTextWithFailingCheckNames(NotificationLevel level)
        {
            var sut = new NimatorResult(DateTime.Now) { Level = level };

            sut.LayerResults.Add(new LayerResult("layer-1", new[] {
                new CheckResult("check-1", NotificationLevel.Okay),
                new CheckResult("check-2", NotificationLevel.Warning),
                new CheckResult("check-3", NotificationLevel.Error),
                new CheckResult("check-4", NotificationLevel.Critical),
            }));

            var result = sut.Message;

            Assert.That(result.ToLowerInvariant(), Does.Contain("check-3"));
            Assert.That(result.ToLowerInvariant(), Does.Contain("check-4"));
        }

        [Test]
        public void RenderPlainText_ForInterestingResults_RendersSensibleText()
        {
            // This is a smoke test. We want to be able to iterate fast on the
            // exact formatting, without having to fix fragile tests all the
            // time (so this test has only "fuzzy" checks).

            var timestamp = new DateTime(2016, 8, 22, 13, 45, 0);
            var sut = new NimatorResult(timestamp) { Finished = timestamp.AddSeconds(5) };

            sut.LayerResults.Add(new LayerResult("layer-1", new[] {
                new CheckResult("check-a", NotificationLevel.Okay),
                new CheckResult("check-b", NotificationLevel.Okay),
            }));

            sut.LayerResults.Add(new LayerResult("layer-2", new[] {
                new CheckResult("check-q", NotificationLevel.Okay),
                new CheckResult("check-w", NotificationLevel.Error),
                new CheckResult("check-e", NotificationLevel.Error),
            }));

            sut.LayerResults.Add(new LayerResult("layer-3", new[] {
                new CheckResult("check-x", NotificationLevel.Okay),
                new CheckResult("check-y", NotificationLevel.Okay),
                new CheckResult("check-z", NotificationLevel.Critical),
            }));

            // Case insensitivity by lowering the result before the assertions.
            var result = sut.RenderPlainText().ToLowerInvariant();
            var firstLine = result.Substring(0, result.IndexOf("\n", StringComparison.Ordinal));

            Assert.That(firstLine, Does.Contain("2016-08-22"));
            Assert.That(firstLine, Does.Contain("13:45:00"));
            Assert.That(firstLine, Does.Contain("13:45:05"));
            Assert.That(firstLine, Does.Contain("critical"));
            Assert.That(result, Does.Contain("layer-1"));
            Assert.That(result, Does.Contain("layer-2"));
            Assert.That(result, Does.Contain("layer-3"));
        }

        [Test]
        public void RenderPlainText_WhenNoResultsAvailable_ReturnsSaneText()
        {
            var sut = new NimatorResult(DateTime.Now);
            sut.LayerResults.Clear();
            var result = sut.RenderPlainText();
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        private static LayerResult TestLayerResult(string name, NotificationLevel level)
        {
            return new LayerResult(name, new[] {
                new CheckResult("dummy-check", level) 
            });
        }
    }
}
