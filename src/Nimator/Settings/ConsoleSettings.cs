using Nimator.Notifiers;

namespace Nimator.Settings
{
    public class ConsoleSettings : NotifierSettings
    {
        public ConsoleSettings()
        {
            this.Threshold = NotificationLevel.Okay;
        }

        public override INotifier ToNotifier()
        {
            return new ConsoleNotifier(this);
        }

        public static NotifierSettings GetExample()
        {
            return new ConsoleSettings();
        }
    }
}
