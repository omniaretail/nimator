using System.Diagnostics.CodeAnalysis;

namespace Nimator.Notifiers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
