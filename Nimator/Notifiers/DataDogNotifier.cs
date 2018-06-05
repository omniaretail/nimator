using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Settings;
using Nimator.Util;
using StatsdClient;

namespace Nimator.Notifiers
{
    class DataDogNotifier : INotifier
    {
        private DataDogSettings settings;

        public DataDogNotifier(DataDogSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Notify(INimatorResult result)
        {
            if (result.Level < settings.Threshold) return;

            if (!result.LayerResults.Any())
            {
                AddStatsdMertics(result.Level,
                    result.GetFirstFailedLayerName(),
                    result.GetFirstFailedCheckName(),
                    GetMessage(result.Message, result.RenderPlainText(settings.Threshold)));
            }

            foreach(var layerResult in result.LayerResults.Where(l => l?.Level >= settings.Threshold))
            {
                var layerName = layerResult.LayerName;
                foreach (var checkResult in layerResult.CheckResults.Where(c => c?.Level >= settings.Threshold))
                {
                    AddStatsdMertics(checkResult.Level,
                    layerName,
                    checkResult.CheckName,
                    GetMessage(checkResult.RenderPlainText()));
                }
            }
        }

        private void AddStatsdMertics(NotificationLevel level, string layerName, string checkName, string message)
        {
            DogStatsd.Increment(statName: checkName);
            DogStatsd.Event(title: checkName,
                text: message,
                sourceType: layerName,
                priority: level.ToString());
        }

        private string GetMessage(params string[] messages)
        {
            var message = string.Join(Environment.NewLine, messages);
            return message.Truncate(settings.MessageLengthLimit);
        }
    }
}
