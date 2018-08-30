using Nimator.Notifiers;
using Nimator.Notifiers.DataDog;
using StatsdClient;

namespace Nimator.Settings
{
    /// <summary>
    /// Settings for creating a <see cref="INotifier"/> that will publish to DataDog: <see href="https://www.datadoghq.com/">datadoghq.com</see>
    /// </summary>
    public class DataDogSettings : NotifierSettings
    {
        /// <summary>
        /// Prefix to be prepended to DataDog events.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The server name of DataDog
        /// </summary>
        public string StatsdServerName { get; set; }

        /// <summary>
        /// The port of DataDog
        /// </summary>
        public int StatsdPort { get; set; } = 8125;

        /// <summary>
        /// The limit of maximum characters number in DataDog message
        /// </summary>
        public int MessageLengthLimit { get; set; } = 5000;

        /// <inheritDoc/>
        public override INotifier ToNotifier()
        {
            ConfigureStatsd();

            var dataDogConverter = new DataDogEventConverter(this);
            return new DataDogNotifier(dataDogConverter, this);
        }

        /// <summary>
        /// Creates a <see cref="SlackSettings"/> example with dummy data.
        /// </summary>
        /// <returns></returns>
        public static NotifierSettings GetExample()
        {
            return new DataDogSettings
            {
                Prefix = "Nimator",
                StatsdServerName = "127.0.0.1",
                StatsdPort = 8125,
                Threshold = NotificationLevel.Warning
            };
        }

        private void ConfigureStatsd()
        {
            DogStatsd.Configure(new StatsdConfig()
            {
                Prefix = Prefix,
                StatsdServerName = StatsdServerName,
                StatsdPort = StatsdPort
            });
        }
    }
}
