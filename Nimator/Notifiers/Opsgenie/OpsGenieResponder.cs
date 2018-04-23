using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nimator.Notifiers.Opsgenie
{
    /// <summary>
    /// Represents the responders for the <see cref="OpsGenieAlertRequest"/>.
    /// </summary>
    public class OpsGenieResponder
    {
        /// <summary>
        /// Identifier for the <see cref="OpsGenieResponderType"/>.
        /// 
        /// LIMIT: -
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// Name for <see cref="OpsGenieResponderType"/>.
        /// 
        /// LIMIT: -
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Username for when the <see cref="OpsGenieResponderType"/> equals to <see cref="OpsGenieResponderType.User"/>.
        ///
        /// LIMIT: -
        /// </summary>
        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        /// <summary>
        /// possible values are <see cref="OpsGenieResponderType.Team"/>, <see cref="OpsGenieResponderType.User"/>, <see cref="OpsGenieResponderType.Escalation"/> and <see cref="OpsGenieResponderType.Schedule"/>.
        ///
        /// LIMIT: -
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public OpsGenieResponderType Type { get; set; }
    }
}
