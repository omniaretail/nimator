namespace Nimator.Settings
{
    /// <summary>
    /// Abstract structure for settings to bootstrap <see cref="INotifier"/> instances.
    /// </summary>
    public abstract class NotifierSettings
    {
        /// <summary>
        /// Constructs default instance.
        /// </summary>
        protected NotifierSettings()
        {
            this.Threshold = NotificationLevel.Error;
        }

        /// <summary>
        /// Threshold at which notifications should start to be sent out by this notifier.
        /// </summary>
        public NotificationLevel Threshold { get; set; }

        /// <summary>
        /// Converts these settings into an <see cref="INotifier"/>, effectively making this method
        /// a mini-composition-root.
        /// </summary>
        public abstract INotifier ToNotifier();
    }
}
