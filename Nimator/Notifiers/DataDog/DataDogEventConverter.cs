using Nimator.Settings;
using Nimator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers.DataDog
{
    class DataDogEventConverter : IDataDogEventConverter
    {
        public const string MetricsName = "Checks";

        private readonly DataDogSettings settings;

        public DataDogEventConverter(DataDogSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IEnumerable<DataDogEvent> Convert(INimatorResult result)
        {
            if (result == null)  throw new ArgumentNullException(nameof(result));

            if (result.Level < settings.Threshold) yield break;

            if (!result.LayerResults.Any())
            {
                yield return ConvertToDataDogEvent(result.Level, 
                    result.GetFirstFailedLayerName(), 
                    result.GetFirstFailedCheckName(), 
                    result.Message, result.RenderPlainText(settings.Threshold));
                yield break;
            }

            var layerResults = result.LayerResults.Where(l => l?.Level >= settings.Threshold);

            foreach (var layerResult in layerResults)
            {
                var layerName = layerResult.LayerName;
                var checkResults = layerResult.CheckResults.Where(c => c?.Level >= settings.Threshold);

                foreach (var checkResult in checkResults)
                {
                    yield return ConvertToDataDogEvent(checkResult.Level, 
                        layerName, 
                        checkResult.CheckName, 
                        checkResult.RenderPlainText());
                }
            }
        }

        private DataDogEvent ConvertToDataDogEvent(NotificationLevel level, string layerName, string checkName, params string[] messages)
        {
            var dataDogEvent = new DataDogEvent
            {
                StatName = MetricsName,
                Title = checkName,
                AlertType = ConvertToAlertType(level)
            };

            var message = string.Join(Environment.NewLine, messages);
            message = message.Truncate(settings.MessageLengthLimit);
            dataDogEvent.Message = message;

            dataDogEvent.AddTag("check", checkName);
            dataDogEvent.AddTag("layer", layerName);
            dataDogEvent.AddTag("level", level.ToString());

            return dataDogEvent;
        }

        private string ConvertToAlertType(NotificationLevel level)
        {
            switch (level)
            {
                case NotificationLevel.Critical:
                case NotificationLevel.Error:
                    return AlertType.Error;
                case NotificationLevel.Warning:
                    return AlertType.Warning;
                case NotificationLevel.Okay:
                default:
                    return AlertType.Info;
            }
        }
    }
}
