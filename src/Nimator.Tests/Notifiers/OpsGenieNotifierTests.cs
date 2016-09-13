using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Settings;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    [TestFixture]
    public class OpsGenieNotifierTests
    {
        [Test]
        public void Constructor_WhenSettingsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OpsGenieNotifier(null));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsApiKeyInvalid_ThrowsException(string input)
        {
            var settings = NewOpsGenieSettingsWith(s => s.ApiKey = input);
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsHeartbeatNameInvalid_ThrowsException(string input)
        {
            var settings = NewOpsGenieSettingsWith(s => s.HeartbeatName = input);
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsTeamNameInvalid_ThrowsException(string input)
        {
            var settings = NewOpsGenieSettingsWith(s => s.TeamName = input);
            var exception = Assert.Throws<ArgumentException>(() => new OpsGenieNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        private OpsGenieSettings NewOpsGenieSettingsWith(Action<OpsGenieSettings> modify)
        {
            var settings = new OpsGenieSettings();
            modify(settings);
            return settings;
        }
    }
}
