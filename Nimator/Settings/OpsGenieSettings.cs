using Nimator.Notifiers;

namespace Nimator.Settings
{
    /// <summary>
    /// Settings for a <see cref="INotifier"/> that calls out to OpsGenie: <see href="https://www.opsgenie.com/">www.opsgenie.com</see>
    /// </summary>
    public class OpsGenieSettings : NotifierSettings
    {
        /// <summary>
        /// Your API key for posting.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The name of the team to receive new Alerts.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// The name of the Heartbeat to keep alive on each cycle.
        /// </summary>
        public string HeartbeatName { get; set; }

        /// <inheritDoc/>
        public override INotifier ToNotifier()
        {
            return new OpsGenieNotifier(this);
        }

        /// <summary>
        /// Creates <see cref="OpsGenieSettings"/> instance with example dummy data.
        /// </summary>
        public static NotifierSettings GetExample()
        {
            return new OpsGenieSettings
            {
                ApiKey = "your-api-key-here",
                TeamName = "TeamNameForAlerts",
                HeartbeatName = "HeartbeatName",
                Threshold = NotificationLevel.Error,
            };
        }
    }
}
