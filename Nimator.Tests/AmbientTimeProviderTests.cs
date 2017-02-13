using System;
using NUnit.Framework;

namespace Nimator
{
    [TestFixture]
    public class AmbientTimeProviderTests
    {
        [Test]
        public void GetNow_ForDefaultInstance_ReturnsNonDefaultTimes()
        {
            var result = AmbientTimeProvider.GetNow();
            Assert.That((DateTime.Now - result).Seconds, Is.LessThan(5), "Expected the ambient time provider to give a result just around Now.");
        }
    }
}
