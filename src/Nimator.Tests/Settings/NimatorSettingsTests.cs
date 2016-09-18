using System;
using System.Linq;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class NimatorSettingsTests
    {
        [Test]
        public void FromJson_ForBasicJsonString_ReturnsBasicSettings()
        {
            var json = "{}";
            var sut = NimatorSettings.FromJson(json);
            Assert.That(sut, Is.Not.Null);
            CollectionAssert.IsEmpty(sut.Notifiers);
        }

        [TestCase(typeof(ConsoleSettings))]
        [TestCase(typeof(OpsGenieSettings))]
        [TestCase(typeof(SlackSettings))]
        public void GetExample_WhenCalled_HasNotifierOfType(Type expectedType)
        {
            var sut = NimatorSettings.GetExample();
            Assert.That(sut.Notifiers.Any(n => n.GetType() == expectedType));
        }

        [Test]
        public void GetExample_WhenCalled_HasMultipleLayers()
        {
            var sut = NimatorSettings.GetExample();
            Assert.That(sut.Layers.Length, Is.GreaterThan(1));
        }

        [Test]
        public void GetExample_WhenCalled_HasAtLeastOneLayerWithMultipleChecks()
        {
            var sut = NimatorSettings.GetExample();
            Assert.That(sut.Layers.Any(l => l.Checks.Length > 1));
        }

        [Test]
        public void GetExample_WhenCalled_GivesLayersWithDistinctNames()
        {
            var sut = NimatorSettings.GetExample();
            var names = sut.Layers.Select(l => l.Name);
            CollectionAssert.AllItemsAreUnique(names);
        }
    }
}
