using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class SlackSettingsTests
    {
        [Test]
        public void ToNotifier_ForSaneDefaultSettings_ReturnsNotifier()
        {
            var sut = new SlackSettings {Url = "http://dummy-url/"};
            var result = sut.ToNotifier();
            Assert.That(result, Is.Not.Null);
        }
    }
}
