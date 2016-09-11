using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class NotifierSettingsTests
    {
        [Test]
        public void Constructor_WhenCalled_SetsSensibleDefaults()
        {
            var sut = new TestableNotifierSettings();
            Assert.That(sut.Threshold, Is.EqualTo(NotificationLevel.Error));
        }

        // So we can test the abstract class by "instantiating it".
        private class TestableNotifierSettings : NotifierSettings
        {
            public override INotifier ToNotifier()
            {
                throw new NotImplementedException();
            }
        }
    }
}
