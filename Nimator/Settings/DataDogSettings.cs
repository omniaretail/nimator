using Nimator.Notifiers;
using StatsdClient;

namespace Nimator.Settings
{
    /// <summary>
    /// Settings for creating a <see cref="INotifier"/> that will publish to DataDog: <see href="https://www.datadoghq.com/">datadoghq.com</see>
    /// </summary>
    public class DataDogSettings : NotifierSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StatsdServerName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StatsdPort { get; set; } = 8125;

        /// <summary>
        /// 
        /// </summary>
        public int MessageLengthLimit { get; set; } = 5000;

        /// <inheritDoc/>
        public override INotifier ToNotifier()
        {
            CongifureStatsd();

            return new DataDogNotifier(this);
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

        private void CongifureStatsd()
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
