using System;
using Nimator.Settings;

namespace Nimator.Notifiers.Opsgenie
{
    /// <inheritdoc/>
    public class OpsGenieAlertConverter : IOpsGenieAlertConverter
    {
        private const int MaxOpsgenieTagLength = 50;
        private const int MaxOpsgenieMessageLength = 130;
        private const int MaxOpsgenieDescriptionLength = 15000;
        
        private readonly OpsGenieSettings settings;

        /// <summary>
        /// Constructs a settings based <see cref="IOpsGenieAlertConverter"/>.
        /// </summary>
        /// <param name="settings">Settings for the alert</param>
        public OpsGenieAlertConverter(OpsGenieSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.TeamName)) throw new ArgumentException("settings.TeamName was not set", nameof(settings.TeamName));

            this.settings = settings;
        }

        /// <inheritdoc/>
        public OpsGenieAlertRequest Convert(INimatorResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            var message = string.IsNullOrEmpty(result.Message) ?
                "Unknown message" :
                Truncate(result.Message, MaxOpsgenieMessageLength);

            var failingLayerName = result.GetFirstFailedLayerName() ?? "UnknownLayer";
            
            return new OpsGenieAlertRequest(message)
            {                
                Alias = "nimator-failure",
                Description = Truncate(result.RenderPlainText(settings.Threshold), MaxOpsgenieDescriptionLength),                
                Responders = new[]
                {
                    new OpsGenieResponder
                    {
                        Type = OpsGenieResponderType.Team,
                        Name = settings.TeamName
                    }
                },
                Tags = new[] { "Nimator", Truncate(failingLayerName, MaxOpsgenieTagLength) }
            };
        }

        private string Truncate(string value, int maxLength)
        {
            if (maxLength < 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (string.IsNullOrEmpty(value)) return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
