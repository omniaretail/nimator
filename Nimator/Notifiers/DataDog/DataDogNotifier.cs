using System.Linq;
using StatsdClient;

namespace Nimator.Notifiers.DataDog
{
    class DataDogNotifier : INotifier
    {
        private readonly IDataDogEventConverter dataDogEventConverter;

        public DataDogNotifier(IDataDogEventConverter dataDogEventConverter)
        {
            this.dataDogEventConverter = dataDogEventConverter ?? throw new System.ArgumentNullException(nameof(dataDogEventConverter));
        }

        public void Notify(INimatorResult result)
        {
            foreach(var dataDogEvent in dataDogEventConverter.Convert(result))
            {
                NotifyDataDogEvent(dataDogEvent);
            }
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
