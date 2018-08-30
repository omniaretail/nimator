using System;
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

            NotifyDataDogHealthMetrics(result);
        }

        private void NotifyDataDogHealthMetrics(INimatorResult result)
        {
            // TODO NJ: Implementation
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
