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
            DogStatsd.Increment(statName: dataDogEvent.CheckName);
            DogStatsd.Event(title: dataDogEvent.CheckName,
                text: dataDogEvent.Message,
                sourceType: dataDogEvent.LayerName,
                priority: dataDogEvent.Level);
        }
    }
}
