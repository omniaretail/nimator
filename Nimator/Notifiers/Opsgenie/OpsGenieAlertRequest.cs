using System;
using Newtonsoft.Json;

namespace Nimator.Notifiers.Opsgenie
{
    /// <summary>
    /// Represents the alert request for the <see cref="IOpsGenieApiClient"/>
    /// </summary>
    public class OpsGenieAlertRequest
    {
       /// <summary>
        /// [MANDATORY] Message of the alert.
        /// 
        /// LIMIT: 130 chars
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; }

        /// <summary>
        /// Used for alert deduplication. A user defined identifier for the alert and there can 
        /// be only one alert with open status with the same alias. Provides ability to assign 
        /// a known id and later use this id to perform additional actions such as log, close, 
        /// attach for the same alert. 
        /// 
        /// LIMIT: 512 chars
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Description field of the alert that is generally used to provide a detailed information about the alert.        
        /// LIMIT: 15000 chars
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Teams, users, escalations and schedules that the alert will be routed to send notifications. 
        /// type field is mandatory for each item, where possible values are team, user, escalation and schedule. 
        /// If the API Key belongs to a team integration, this field will be overwritten with the owner team. 
        /// Either id or name of each responder should be provided.You can refer below for example values.
        /// 
        /// LIMIT: 50 teams, users, escalations or schedules
        /// </summary>
        [JsonProperty("responders")]
        public OpsGenieResponder[] Responders { get; set; }

        /// <summary>
        /// A comma separated list of labels attached to the alert. You can overwrite Quiet Hours 
        /// setting for urgent alerts by adding OverwritesQuietHours tag. Tags which are exceeding 
        /// the number limit are ignored. Tag names which are longer than length limit are shortened.
        /// 
        /// LIMIT: 20 tags, 50 chars each
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Contructs an alert request for <see href="https://www.opsgenie.com/">opsgenie</see>.
        /// </summary>
        /// <param name="message">Message of the alert</param>
        public OpsGenieAlertRequest(string message)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentException("Cannot be empty", nameof(message));

            Message = message;
        }
    }
}
