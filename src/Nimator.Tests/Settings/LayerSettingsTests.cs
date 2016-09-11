using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator.Settings
{
    [TestFixture]
    public class LayerSettingsTests
    {
        [Test]
        public void Constructor_WhenCalled_SetsSensibleDefaults()
        {
            var sut = new LayerSettings();
            Assert.That(sut.Checks, Is.Not.Null.And.Empty);
        }
    }
}
