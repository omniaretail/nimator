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
                NotifyDataDog(dataDogEvent);
            }
        }

        protected virtual void NotifyDataDog(DataDogEvent dataDogEvent)
        {
            var tags = new[] { dataDogEvent.CheckName, dataDogEvent.LayerName, dataDogEvent.Level };

            DogStatsd.Increment(statName: dataDogEvent.StatName, tags: tags);
            DogStatsd.Event(title: dataDogEvent.CheckName,
                text: dataDogEvent.Message,
                alertType: dataDogEvent.AlertType,
                tags: tags);
        }
    }
}
