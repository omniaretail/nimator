using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers
{
    internal class OpsGenieCreateAlertRequest : OpsGenieRequest
    {
        public OpsGenieCreateAlertRequest(string apiKey, string message)
            : base(apiKey)
        {
            this.message = message;
        }

        /// <summary>
        /// [MANDATORY] Alert text limited to 130 characters
        /// 
        /// LIMIT: 130 chars
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Used for alert deduplication. A user defined identifier for the alert and there can 
        /// be only one alert with open status with the same alias. Provides ability to assign 
        /// a known id and later use this id to perform additional actions such as log, close, 
        /// attach for the same alert. 
        /// 
        /// LIMIT: 512 chars
        /// </summary>
        public string alias { get; set; }

        /// <summary>
        /// This field can be used to provide a detailed description of the alert, anything that
        /// may not have fit in the Message field.
        /// 
        /// LIMIT: 15000 chars
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// List of team names which will be responsible for the alert. Team escalation policies
        /// are run to calculate which users will receive notifications. Teams which are exceeding
        /// the limit are ignored.
        /// 
        /// LIMIT: 50 teams
        /// </summary>
        public string[] teams { get; set; }

        /// <summary>
        /// A comma separated list of labels attached to the alert. You can overwrite Quiet Hours 
        /// setting for urgent alerts by adding OverwritesQuietHours tag. Tags which are exceeding 
        /// the number limit are ignored. Tag names which are longer than length limit are shortened.
        /// 
        /// LIMIT: 20 tags, 50 chars each
        /// </summary>
        public string[] tags { get; set; }
    }
}
