using System;
using Nimator.Settings;
using System.Web;
using Newtonsoft.Json;

namespace Nimator.Notifiers.Opsgenie
{
    /// <inheritdoc/>
    public class OpsGenieApiClient : IOpsGenieApiClient
    {
        private const string AlertUrl = "https://api.opsgenie.com/v2/alerts";
        private const string HeaderApiKeyName = "Authorization";
        private const string ContentTypeJson = "application/json";
        
        private readonly IHttpRequestHandler httpHandler;

        private string heartbeatUrl;
        private string headerApiKeyValue;

        /// <summary>
        /// Constructs an actual <see cref="IOpsGenieApiClient"/>.
        /// </summary>
        /// <param name="httpHandler">The <see cref="IHttpRequestHandler"/> to use</param>
        /// <param name="settings">The <see href="https://www.opsgenie.com/">opsgenie</see> API settings</param>
        public OpsGenieApiClient(
            IHttpRequestHandler httpHandler,
            OpsGenieSettings settings)
        {
            if (httpHandler == null) throw new ArgumentNullException(nameof(httpHandler));

            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.ApiKey)) throw new ArgumentException("settings.ApiKey was not set", nameof(settings.ApiKey));
            if (string.IsNullOrWhiteSpace(settings.HeartbeatName)) throw new ArgumentException("settings.HeartbeatName was not set", nameof(settings.HeartbeatName));

            this.httpHandler = httpHandler;

            headerApiKeyValue = $"GenieKey {settings.ApiKey}";

            var encodedHeartbeatName = HttpUtility.UrlEncode(settings.HeartbeatName);
            heartbeatUrl = $"https://api.opsgenie.com/v2/heartbeats/{encodedHeartbeatName}/ping";
        }

        /// <inheritdoc/>
        public void SendHeartbeat()
        {
            var request = httpHandler.CreateRequest(heartbeatUrl);

            request.Method = "GET";
            request.Headers[HeaderApiKeyName] = headerApiKeyValue;

            httpHandler.HandleRequest(request);            
        }

        /// <inheritdoc/>
        public void SendAlert(OpsGenieAlertRequest alert)
        {                        
            var request = httpHandler.CreateRequest(AlertUrl);

            request.Method = "POST";
            request.Headers[HeaderApiKeyName] = headerApiKeyValue;
            request.Accept = ContentTypeJson;
            request.ContentType = ContentTypeJson;
            request.KeepAlive = false;
            
            httpHandler.HandleRequest(request, JsonConvert.SerializeObject(alert));
        }       


    }
}
