using System;
using Nimator.Settings;

namespace Nimator.Notifiers
{
    internal class ConsoleNotifier : INotifier
    {
        private readonly ConsoleSettings settings;
        private readonly Action<string> writeLine;

        public ConsoleNotifier(ConsoleSettings settings, Action<string> writeLine)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (writeLine == null) throw new ArgumentNullException(nameof(writeLine));

            this.settings = settings;
            this.writeLine = writeLine;
        }

        public void Notify(INimatorResult result)
        {
            if (result.Level >= settings.Threshold)
            {
                writeLine(result.RenderPlainText(settings.Threshold));
            }
        }
    }
}
