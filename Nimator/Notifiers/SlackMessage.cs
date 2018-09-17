using System;
using System.Linq;
using Newtonsoft.Json;

namespace Nimator.Notifiers
{
    internal class SlackMessage
    {
        public const string ENV_PREFIX = "Environment: ";

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public SlackMessageAttachment[] SlackMessageAttachments { get; set; }

        public SlackMessage(INimatorResult result, NotificationLevel minLevelForDetails, string sourceEnvironment)
        {
            // Because "text" will be shown (a) full-width and (b) full height without
            // a 'Show More...' link, we prefer that to use for the full description.
            // The "attachments" will then be a simpler things, drawing attention with
            // specific coloring and icons.

            Text = GetEnvironmentMessage(sourceEnvironment)
                   + result.Message
                   + $":\n```{result.RenderPlainText(minLevelForDetails)}```";

            SlackMessageAttachments = new[] 
            {
                new SlackMessageAttachment
                {
                    Text = CallToActionForLevel(result.Level),
                    Color = GetHexForLevel(result.Level)
                }
            };
        }

        public void AddAttachment(string addendum)
        {
            SlackMessageAttachments = SlackMessageAttachments.Union(new[] 
            {
                new SlackMessageAttachment
                {
                    Text = addendum,
                    Color = "#00A2E8",
                }
            }).ToArray();
        }

        private string GetEnvironmentMessage(string sourceEnvironment)
            => string.IsNullOrWhiteSpace(sourceEnvironment)
                ? string.Empty
                : $"{ENV_PREFIX}{sourceEnvironment}.{Environment.NewLine}";

        private static string CallToActionForLevel(NotificationLevel level)
        {
            switch (level)
            {
                case NotificationLevel.Okay:
                    return ":white_check_mark: Everything is just fine!";
                case NotificationLevel.Warning:
                    return ":warning: Careful: warnings are errors of the future!";
                case NotificationLevel.Error:
                    return ":x: You really should take some action!";
                case NotificationLevel.Critical:
                    return ":fire: Stuff is on fire! (Or monitoring's broken...)";
                default:
                    return ":grey_question: It is quite unclear what the status is...";
            }
        }

        private static string GetHexForLevel(NotificationLevel level)
        {
            switch (level)
            {
                case NotificationLevel.Okay:
                    return "#22B14C";
                case NotificationLevel.Warning:
                    return "#FFD427";
                case NotificationLevel.Error:
                    return "#FF5527";
                case NotificationLevel.Critical:
                default:
                    return "#000000";
            }
        }
    }

    internal class SlackMessageAttachment
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("mrkdwn_in")]
        public string[] UseMarkdownTrigger => new[] { "text" };

        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
