using System;
using System.Collections.Generic;

namespace Nimator
{
    internal class CriticalNimatorResult : INimatorResult
    {
        private readonly string fullText;

        public CriticalNimatorResult(string message, string fullText)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Provided message was null or whitespace, but must be sensible or consumers will not understand what is going on.", nameof(message));

            // We explicitly do not use the Ambient DateTime Provider here, because
            // in edge cases that might *be* the cause for this critical result.
            this.Started = DateTime.Now;
            this.Finished = DateTime.Now;

            this.Level = NotificationLevel.Critical;
            this.LayerResults = new List<LayerResult>();
            this.Message = message;
            this.fullText = string.IsNullOrWhiteSpace(fullText) ? message : fullText;
        }

        public DateTime Started { get; }

        public DateTime Finished { get; }

        public NotificationLevel Level { get; }

        public IList<LayerResult> LayerResults { get; }

        public string Message { get; }

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
