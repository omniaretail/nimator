namespace Nimator.Settings
{
    public abstract class NotifierSettings
    {
        protected NotifierSettings()
        {
            this.Threshold = NotificationLevel.Error;
        }

        public NotificationLevel Threshold { get; set; }

        public abstract INotifier ToNotifier();
    }
}
