namespace Nimator
{
    /// <summary>
    /// Settings for checks that don't do anything besides returning a specific result after an (optional) delay.
    /// </summary>
    public class NoopCheckSettings : ICheckSettings
    {
        /// <summary>
        /// Constructs default settings.
        /// </summary>
        public NoopCheckSettings()
        {
            LevelToSimulate = NotificationLevel.Okay;
        }

        /// <summary>
        /// How long the <see cref="NoopCheck"/> should simulate being busy.
        /// </summary>
        public int DelayResultInMs { get; set; }

        /// <summary>
        /// The <see cref="NotificationLevel"/> to return when run.
        /// </summary>
        public NotificationLevel LevelToSimulate { get; set; }

        /// <inheritDoc/>
        public ICheck ToCheck()
        {
            return new NoopCheck(this);
        }
    }
}
