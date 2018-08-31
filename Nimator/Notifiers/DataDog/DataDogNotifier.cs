using System;
using System.Collections.Generic;
using System.Linq;
using Nimator.Settings;
using StatsdClient;

namespace Nimator.Notifiers.DataDog
{
    class DataDogNotifier : INotifier
    {
        private readonly IDataDogEventConverter _dataDogEventConverter;
        private readonly DataDogSettings _settings;

        public DataDogNotifier(IDataDogEventConverter dataDogEventConverter, DataDogSettings settings)
        {
            if (dataDogEventConverter == null) throw new ArgumentNullException(nameof(dataDogEventConverter));
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            _dataDogEventConverter = dataDogEventConverter;
            _settings = settings;
        }

        public void Notify(INimatorResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (result.Level >= _settings.Threshold)
            {
                foreach (var dataDogEvent in _dataDogEventConverter.Convert(result))
                {
                    NotifyDataDogEvent(dataDogEvent);
                }
            }

            SendDataDogHealthMetrics(result);
        }

        private void SendDataDogHealthMetrics(INimatorResult nimatorResult)
        {
            
            foreach (var resultLayerResult in nimatorResult.LayerResults)
            {
                foreach (var checkResult in resultLayerResult.CheckResults)
                {
                    string[] tags = new string[]
                    {
                        $"layer:{resultLayerResult.LayerName}",
                        $"level:{checkResult.Level.ToString()}"
                    };

                    SendCheckResultToDataDog(checkResult, tags);
                    SendIsHealthyToDataDog(checkResult, tags);
                }
            }            
        }

        private void SendIsHealthyToDataDog(ICheckResult checkResult, string[] tags)
        {
            var checkResultCheckName = checkResult.CheckName;

            string isHealthyStateName = $"check.{checkResultCheckName}.isHealthy";
            int isHealthyValue = Convert.ToInt32(checkResult.Level == NotificationLevel.Okay);
            NotifyDataDogGauge(isHealthyStateName, isHealthyValue.ToString(), tags);
        }

        private void SendCheckResultToDataDog(ICheckResult checkResult, string[] tags)
        {
            string resultStatName = $"check.{checkResult.CheckName}.result";
            int resultValue = TranslateLevel(checkResult.Level);
            NotifyDataDogGauge(resultStatName, resultValue.ToString(), tags);
        }

        private int TranslateLevel(NotificationLevel checkResultLevel)
        {
            switch (checkResultLevel)
            {
                case NotificationLevel.Okay:
                    return 0;
                case NotificationLevel.Warning:
                    return 1;
                case NotificationLevel.Error:
                    return 2;
                case NotificationLevel.Critical:
                    return 3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void NotifyDataDogGauge(string statName, string value, string[] tags)
        {
            DogStatsd.Gauge(statName: statName, value: value, tags: tags);
        }

        protected virtual void NotifyDataDogEvent(DataDogEvent dataDogEvent)
        {
            DogStatsd.Increment(statName: dataDogEvent.StatName, tags: dataDogEvent.Tags);
            DogStatsd.Event(title: dataDogEvent.Title,
                text: dataDogEvent.Message,
                alertType: dataDogEvent.AlertType,
                tags: dataDogEvent.Tags);
        }
    }    
}
