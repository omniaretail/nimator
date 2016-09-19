using System;
using Nimator.Settings;

namespace Nimator.Notifiers
{
    internal class ConsoleNotifier : INotifier
    {
        private readonly ConsoleSettings settings;

        public ConsoleNotifier(ConsoleSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            this.settings = settings;
        }

        public void Notify(INimatorResult result)
        {
            if (result.Level >= settings.Threshold)
            {
                Console.WriteLine(result.RenderPlainText(settings.Threshold));
            }
        }
    }
}
