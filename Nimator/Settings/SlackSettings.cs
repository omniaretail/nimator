using Nimator.Notifiers;

namespace Nimator.Settings
{
    /// <summary>
    /// Settings for creating a <see cref="INotifier"/> that will publish to Slack: <see href="https://slack.com/">slack.com</see>
    /// </summary>
    public class SlackSettings : NotifierSettings
    {
        /// <summary>
        /// The webhook integration url to post to.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A number of seconds to wait for any subsequent post.
        /// </summary>
        public int DebounceTimeInSecs { get; set; }

        /// <summary>
        /// Source environment to show in slack message.
        /// Optional - use when you have one channel for several environments.
        /// </summary>
        public string SourceEnvironment { get; set; }

        /// <inheritDoc/>
        public override INotifier ToNotifier()
        {
            return new SlackNotifier(this);
        }

        /// <summary>
        /// Creates a <see cref="SlackSettings"/> example with dummy data.
        /// </summary>
        public static NotifierSettings GetExample()
        {
            return new SlackSettings
            {
                Url = "https://hooks.slack.com/services/your/integrationUrl/here",
                DebounceTimeInSecs = 3600,
                Threshold = NotificationLevel.Error
            };
        }
    }
}
