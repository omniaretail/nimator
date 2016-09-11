using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers
{
    internal class OpsGenieRequest
    {
        public OpsGenieRequest(string apiKey)
        {
            this.apiKey = apiKey;
        }

        /// <summary>
        /// [MANDATORY] API key is used for authenticating API requests
        /// </summary>
        public string apiKey { get; set; }
    }
}
