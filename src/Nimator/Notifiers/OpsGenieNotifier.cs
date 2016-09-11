using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nimator;
using Nimator.Settings;
using Nimator.Util;

namespace Nimator.Notifiers
{
    internal class OpsGenieNotifier : INotifier
    {
        private const int maxOpsgenieTagLength = 50;
        private const int maxOpsgenieMessageLength = 130;
        private const string alertUrl = "https://api.opsgenie.com/v1/json/alert";
        private const string heartbeatUrl = "https://api.opsgenie.com/v1/json/heartbeat/send";
        private readonly OpsGenieSettings settings;

        public OpsGenieNotifier(OpsGenieSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (string.IsNullOrWhiteSpace(settings.ApiKey)) throw new ArgumentException("settings.ApiKey was not set");
            if (string.IsNullOrWhiteSpace(settings.HeartbeatName)) throw new ArgumentException("settings.HeartbeatName was not set");
            if (string.IsNullOrWhiteSpace(settings.TeamName)) throw new ArgumentException("settings.TeamName was not set");

            this.settings = settings;
        }
        
        public void Notify(INimatorResult result)
        {
            SendHeartbeat();

            if (result.Level >= settings.Threshold)
            {
                NotifyFailureResult(result);
            }
        }

        private void SendHeartbeat()
        {
            var request = new OpsGenieHeartbeatRequest(this.settings.ApiKey, this.settings.HeartbeatName);
            SimpleRestUtils.PostToRestApi(heartbeatUrl, request);
        }

        private void NotifyFailureResult(INimatorResult result)
        {
            var failingLayerName = (result.GetFirstFailedLayerName() ?? "UnknownLayer").Truncate(maxOpsgenieTagLength);
            var message = result.Message.Truncate(maxOpsgenieMessageLength);

            var request = new CreateAlertRequest(this.settings.ApiKey, message)
            {
                alias = "nimator-failure",
                description = result.RenderPlainText(),
                teams = new[] { this.settings.TeamName },
                tags = new[] { "Nimator", failingLayerName }
            };

            SimpleRestUtils.PostToRestApi(alertUrl, request);
        }
    }
}
