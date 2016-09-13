using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class NimatorTests
    {
        [Test]
        public void FromSettings_ForMostBasicJson_ReturnsNimator()
        {
            // Bit of a smoke test, but better than nothing...
            var json = "{}";
            var result = Nimator.FromSettings(json);
            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public void FromSettings_WhenGivenExampleAsJson_ReturnsNimator()
        {
            // Bit of a smoke test, but better than nothing...
            var example = NimatorSettings.GetExample();
            var json = example.ToJson();
            var result = Nimator.FromSettings(json);
            Assert.That(result, Is.Not.Null);
        }
    }
}
