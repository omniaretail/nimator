using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Util
{
    /// <summary>
    /// Represents a custom <see cref="WebClient"/> extension with some extra modification.
    /// </summary>
    public class ExtendedWebClient : WebClient
    {
        private readonly TimeSpan timeout;

        /// <summary>
        /// Creates a new instance of the <see cref="ExtendedWebClient"/> class.
        /// </summary>
        /// <param name="timeout">Maximal duration of request(s).</param>
        public ExtendedWebClient(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Overrides the <see cref="GetWebRequest"/> method.
        /// </summary>
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

            request.Timeout = (int)this.timeout.TotalMilliseconds;
            request.AllowWriteStreamBuffering = false;

            return request;
        }
    }
}
