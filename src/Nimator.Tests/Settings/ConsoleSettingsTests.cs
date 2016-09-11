using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture] 
    public class ConsoleSettingsTests
    {
        [Test]
        public void Constructor_WhenCalled_SetsSensibleDefaults()
        {
            var sut = new ConsoleSettings();
            Assert.That(sut.Threshold, Is.EqualTo(NotificationLevel.Okay));
        }
    }
}
