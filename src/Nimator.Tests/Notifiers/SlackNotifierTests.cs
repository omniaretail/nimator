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
    public class SlackNotifierTests
    {
        [Test]
        public void Constructor_WhenSettingsNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new SlackNotifier(null));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Constructor_WhenSettingsUrlInvalid_ThrowsException(string url)
        {
            var settings = new SlackSettings {Url = url};
            var exception = Assert.Throws<ArgumentException>(() => new SlackNotifier(settings));
            Assert.That(exception.ParamName, Is.EqualTo("settings"));
        }
    }
}
