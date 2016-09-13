using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Settings;
using Nimator.Util;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    [TestFixture]
    public abstract class NotifierTests
    {
        protected int numberOfRestCalls;
        protected string mostRecentRestUrlCalled;
        protected object mostRecentRestPayload;

        [SetUp]
        public virtual void SetUp()
        {
            numberOfRestCalls = 0;
            mostRecentRestUrlCalled = null;
            mostRecentRestPayload = null;

            SimpleRestUtils.SetPostToRestApiAction((url, payload) =>
            {
                numberOfRestCalls++;
                mostRecentRestUrlCalled = url;
                mostRecentRestPayload = payload;
            });
        }
    }
}
