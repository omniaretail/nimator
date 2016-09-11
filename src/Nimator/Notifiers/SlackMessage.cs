using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Notifiers
{
    internal class SlackMessage
    {
        public SlackMessage(INimatorResult result)
        {
            // Because "text" will be shown (a) full-width and (b) full height without
            // a 'Show More...' link, we prefer that to use for the full description.
            // The "attachment" will then be a simple title and image to draw the 
            // attention visually.

            text = "```" + result.RenderPlainText() + "```";

            attachments = new[] 
            {
                new Attachment
                {
                    text = GetEmojiForLevel(result.Level) + " " + result.Message,
                    color = GetHexForLevel(result.Level)
                }
            };
        }

        public void AddAttachment(string addendum)
        {
            attachments = attachments.Union(new[] 
            {
                new Attachment
                {
                    text = addendum,
                    color = "#00A2E8",
                }
            }).ToArray();
        }

        public string text { get; set; }

        public Attachment[] attachments { get; set; }

        private static string GetEmojiForLevel(NotificationLevel level)
        {
            switch (level)
            {
                case NotificationLevel.Okay:
                    return ":white_check_mark: ";
                case NotificationLevel.Warning:
                    return ":warning:";
                case NotificationLevel.Error:
                    return ":x:";
                case NotificationLevel.Critical:
                    return ":fire: :fire: :fire:";
                default:
                    return ":grey_question:";
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

    public class Attachment
    {
        public string text { get; set; }

        public string[] mrkdwn_in { get { return new[] { "text" }; } }

        public string color { get; set; }
    }
}
