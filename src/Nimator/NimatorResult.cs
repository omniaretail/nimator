using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    internal class NimatorResult : INimatorResult
    {
        private const NotificationLevel ThresholdLevel = NotificationLevel.Error;

        public NimatorResult(DateTime started)
        {
            this.Started = started;
            this.LayerResults = new List<LayerResult>();
        }

        private NotificationLevel? overriddenNotificationlevel = null;

        public NotificationLevel Level
        {
            get
            {
                return this.overriddenNotificationlevel ?? (NotificationLevel)LayerResults.Max(r => (int)r.Level);
            }
            set
            {
                this.overriddenNotificationlevel = value;
            }
        }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public string Message
        {
            get
            {
                switch (this.Level)
                {
                    case NotificationLevel.Okay:
                        return "OKAY: All checks have been executed.";
                    case NotificationLevel.Warning:
                        return "WARNINGS: One or more individual checks had warnings.";
                    case NotificationLevel.Error:
                    case NotificationLevel.Critical:
                        return string.Format(
                            "Failure in [{0}], checks [{1}].",
                            string.Join(", ", GetFailingLayerNames()),
                            string.Join(", ", GetFailingCheckNames())
                        );
                    default:
                        return "Result with unknown severity";
                }
            }
        }

        public IList<LayerResult> LayerResults { get; private set; }

        public string[] GetFailingLayerNames()
        {
            return LayerResults
                .Where(l => l.Level >= ThresholdLevel)
                .Select(l => l.LayerName)
                .ToArray();
        }

        public string[] GetFailingCheckNames()
        {
            return LayerResults
                .SelectMany(l => l.CheckResults)
                .Where(c => c.Level >= ThresholdLevel)
                .Select(c => c.CheckName)
                .ToArray();
        }

        public string GetFirstFailedLayerName()
        {
            return LayerResults
                .Where(l => l.Level >= ThresholdLevel)
                .Select(l => l.LayerName)
                .FirstOrDefault()
                ?? string.Empty;
        }

        public string RenderPlainText()
        {
            var sb = new StringBuilder();

            sb.AppendFormat(
                "{0} between {1} and {2} (on {3})\n{4}\n",
                Level.ToString().ToUpperInvariant(),
                Started.ToString("HH:mm:ss.fff"),
                Finished.ToString("HH:mm:ss.fff"),
                Finished.ToString("yyyy-MM-dd"),
                this.Message
            );

            foreach (var layerResult in LayerResults)
            {
                sb.AppendFormat(" - {0}\n", layerResult.ToString());
            }

            return sb.ToString();
        }
    }
}
