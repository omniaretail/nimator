using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers
{
    internal class OpsGenieHeartbeatRequest : OpsGenieRequest
    {
        public OpsGenieHeartbeatRequest(string apiKey, string name)
            : base(apiKey)
        {
            this.name = name;
        }

        /// <summary>
        /// Name of the heartbeat.
        /// </summary>
        public string name { get; set; }
    }
}
