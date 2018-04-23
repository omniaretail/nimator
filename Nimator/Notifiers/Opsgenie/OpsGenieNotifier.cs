using System;
using Nimator.Settings;

namespace Nimator.Notifiers.Opsgenie
{
    /// <summary>
    /// Represents the notifier that can distribute <see cref="INimatorResult"/> to <see href="https://www.opsgenie.com/">opsgenie</see>.
    /// </summary>
    public class OpsGenieNotifier : INotifier
    {
        private readonly IOpsGenieApiClient client;
        private readonly IOpsGenieAlertConverter converter;
        private readonly OpsGenieSettings settings;

        /// <summary>
        /// Construct an <see cref="INotifier"/> for <see href="https://www.opsgenie.com/">opsgenie</see>.
        /// </summary>
        /// <param name="client">Api client</param>
        /// <param name="converter">From <see cref="INimatorResult"/> to <see cref="OpsGenieAlertRequest"/></param>
        /// <param name="settings">For threshold configuration</param>
        public OpsGenieNotifier(
            IOpsGenieApiClient client,
            IOpsGenieAlertConverter converter,
            OpsGenieSettings settings)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            this.client = client;
            this.converter = converter;
            this.settings = settings;
        }

        /// <inheritdoc/>
        public void Notify(INimatorResult result)
        {
            client.SendHeartbeat();

            if (result.Level >= settings.Threshold)
            {
                var request = converter.Convert(result);
                client.SendAlert(request);
            }
        }
    }
}
