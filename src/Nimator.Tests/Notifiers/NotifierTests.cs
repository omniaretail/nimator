using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Util;
using NUnit.Framework;

namespace Nimator.Notifiers
{
    [TestFixture]
    public abstract class NotifierTests
    {
        protected int NumberOfRestCalls { get; set; }
        protected string MostRecentRestUrlCalled { get; set; }
        protected object MostRecentRestPayload { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            NumberOfRestCalls = 0;
            MostRecentRestUrlCalled = null;
            MostRecentRestPayload = null;

            SimpleRestUtils.SetPostToRestApiAction((url, payload) =>
            {
                NumberOfRestCalls++;
                MostRecentRestUrlCalled = url;
                MostRecentRestPayload = payload;
            });
        }
    }
}
