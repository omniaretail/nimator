namespace Nimator.Notifiers.Opsgenie
{
    /// <summary>
    /// Represents the functionality of the <see href="https://www.opsgenie.com/">opsgenie</see> api.
    /// </summary>
    public interface IOpsGenieApiClient
    {
        /// <summary>
        /// Sending a heartbeat request.
        /// </summary>
        void SendHeartbeat();

        /// <summary>
        /// Sending an alert.
        /// </summary>
        /// <param name="request">Alert to send</param>
        void SendAlert(OpsGenieAlertRequest request);
    }
}
