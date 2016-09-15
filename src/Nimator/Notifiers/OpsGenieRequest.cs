using System.Diagnostics.CodeAnalysis;

namespace Nimator.Notifiers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
