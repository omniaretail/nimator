using Newtonsoft.Json;

namespace Nimator.Notifiers
{
    internal class OpsGenieRequest
    {
        public OpsGenieRequest(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        /// <summary>
        /// [MANDATORY] API key is used for authenticating API requests
        /// </summary>
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}
