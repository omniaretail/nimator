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
        private DataDogSettings settings;

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
                yield return new DataDogEvent
                {
                    Level = result.Level.ToString(),
                    LayerName = result.GetFirstFailedLayerName(),
                    CheckName = result.GetFirstFailedCheckName(),
                    Message = GetMessage(result.Message, result.RenderPlainText(settings.Threshold))
                };
                yield break;
            }

            foreach (var layerResult in result.LayerResults.Where(l => l?.Level >= settings.Threshold))
            {
                var layerName = layerResult.LayerName;
                foreach (var checkResult in layerResult.CheckResults.Where(c => c?.Level >= settings.Threshold))
                {
                    yield return new DataDogEvent
                    {
                        Level = checkResult.Level.ToString(),
                        LayerName = layerName,
                        CheckName = checkResult.CheckName,
                        Message = GetMessage(checkResult.RenderPlainText())
                    };
                }
            }
        }

        private string GetMessage(params string[] messages)
        {
            var message = string.Join(Environment.NewLine, messages);
            return message.Truncate(settings.MessageLengthLimit);
        }
    }
}
