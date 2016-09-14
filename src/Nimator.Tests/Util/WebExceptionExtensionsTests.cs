using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Nimator.Util
{
    [TestFixture]
    public class WebExceptionExtensionsTests
    {
        private const string FallbackExceptionMessage = "HttpStatus Not Available";

        // It would be nice to have a simple test to check that 
        // a ProtocolError (e.g. a HttpStatus NotFound) would
        // lead to a certain string, but alas: it's requires 
        // quite some work to mock that stuff. See:
        // http://stackoverflow.com/a/9823224/419956

        [Test]
        public void GetHttpStatus_WhenCalledOnNull_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((WebException)null).GetHttpStatus());
            
            Assert.That(exception.ParamName, Is.EqualTo("exception"));
        }

        [Test]
        public void GetHttpStatus_WhenExceptionIsNoHttpError_ReturnsConstText()
        {
            var subject = new WebException("msg", new Exception(), WebExceptionStatus.Timeout, null);
            var result = subject.GetHttpStatus();
            Assert.That(result, Is.EqualTo(FallbackExceptionMessage));
        }

        [Test]
        public void GetHttpStatus_WhenExceptionResponseIsNoHttpWebResponse_ReturnsConstText()
        {
            var subject = new WebException("msg", new Exception(), WebExceptionStatus.ProtocolError, null);
            var result = subject.GetHttpStatus();
            Assert.That(result, Is.EqualTo(FallbackExceptionMessage));
        }
    }
}
