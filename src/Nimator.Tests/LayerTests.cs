using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Nimator;

namespace Nimator
{
    [TestFixture]
    public class LayerTests
    {
        [Test]
        public void Constructor_WhenGivenNoName_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Layer(null, new ICheck[0]));
            Assert.Throws<ArgumentException>(() => new Layer("", new ICheck[0]));
            Assert.Throws<ArgumentException>(() => new Layer("     ", new ICheck[0]));
        }

        [Test]
        public void RunChecks_WhenLayerHasChecks_RunsThroughAllChecks()
        {
            var result1 = new CheckResult("check A", NotificationLevel.Okay);
            var check1 = new Mock<ICheck>();
            check1.Setup(c => c.RunAsync()).Returns(result1.AsTaskResult());

            var result2 = new CheckResult("check B", NotificationLevel.Okay);
            var check2 = new Mock<ICheck>();
            check2.Setup(c => c.RunAsync()).Returns(result2.AsTaskResult());

            var layer = new Layer("Layer 1", new ICheck[] { check1.Object, check2.Object });

            var result = layer.Run();

            check1.Verify(c => c.RunAsync());
            check2.Verify(c => c.RunAsync());
        }

        [Test]
        public void RunChecks_WhenLayerHasChecks_AccumulatesResults()
        {
            var result1 = new CheckResult("check A", NotificationLevel.Okay);
            var check1 = new Mock<ICheck>();
            check1.Setup(c => c.RunAsync()).Returns(result1.AsTaskResult());

            var result2 = new CheckResult("check B", NotificationLevel.Okay);
            var check2 = new Mock<ICheck>();
            check2.Setup(c => c.RunAsync()).Returns(result2.AsTaskResult());

            var layer = new Layer("Layer 1", new ICheck[] { check1.Object, check2.Object });

            var result = layer.Run();

            Assert.That(result.CheckResults.First(), Is.EqualTo(result1));
            Assert.That(result.CheckResults.Last(), Is.EqualTo(result2));
            Assert.That(result.CheckResults.Count(), Is.EqualTo(2));
            Assert.That(result.LayerName, Is.EqualTo("Layer 1"));
        }

        [Test]
        public void RunChecks_WhenLayerHasNoChecks_ReturnsWarning()
        {
            var layer = new Layer("Layer 1", new ICheck[0]);
            var result = layer.Run();
            Assert.That(result.Level, Is.EqualTo(NotificationLevel.Warning));
            Assert.That(result.ToString().ToLowerInvariant(), Does.Contain("warning"));
            Assert.That(result.ToString().ToLowerInvariant(), Does.Contain("0 check"));
        }
    }
}
