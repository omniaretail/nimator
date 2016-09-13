using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class OpsGenieSettingsTests
    {
        [Test]
        public void ToNotifier_ForSaneDefaultSettings_ReturnsNotifier()
        {
            var sut = new OpsGenieSettings
            {
                ApiKey = "dummy-key",
                HeartbeatName = "dummy-name",
                TeamName = "dummy-team",
            };
            var result = sut.ToNotifier();
            Assert.That(result, Is.Not.Null);
        }
    }
}
