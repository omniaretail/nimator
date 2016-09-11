using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    internal class CriticalNimatorResult : INimatorResult
    {
        private readonly string fullText;

        public CriticalNimatorResult(string message, string fullText)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Provided message was null or whitespace, but must be sensible or consumers will not understand what is going on.", "message");

            // We explicitly do not use the Ambient DateTime Provider here, because
            // in edge cases that might *be* the cause for this critical result.
            this.Started = DateTime.Now;
            this.Finished = DateTime.Now;

            this.Level = NotificationLevel.Critical;
            this.LayerResults = new List<LayerResult>();
            this.Message = message;
            this.fullText = string.IsNullOrWhiteSpace(fullText) ? message : fullText;
        }

        public DateTime Started { get; private set; }

        public DateTime Finished { get; private set; }

        public NotificationLevel Level { get; private set; }

        public IList<LayerResult> LayerResults { get; private set; }

        public string Message { get; private set; }

        public string GetFirstFailedLayerName()
        {
            return "UnknownLayer";
        }

        public string[] GetFailingLayerNames()
        {
            return new [] { GetFirstFailedLayerName() };
        }

        public string[] GetFailingCheckNames()
        {
            return new[] { "UnknownCheck" };
        }

        public string RenderPlainText()
        {
            return fullText;
        }
    }
}
