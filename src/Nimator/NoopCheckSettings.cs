namespace Nimator
{
    public class NoopCheckSettings : ICheckSettings
    {
        public NoopCheckSettings()
        {
            LevelToSimulate = NotificationLevel.Okay;
        }

        public int DelayResultInMs { get; set; }

        public NotificationLevel LevelToSimulate { get; set; }

        public ICheck ToCheck()
        {
            return new NoopCheck(this);
        }
    }
}
