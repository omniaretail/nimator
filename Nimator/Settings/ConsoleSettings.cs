﻿using System;
using Nimator.Notifiers;

namespace Nimator.Settings
{
    /// <summary>
    /// Settings for a <see cref="ConsoleNotifier"/>.
    /// </summary>
    public class ConsoleSettings : NotifierSettings
    {
        /// <summary>
        /// Constructs default settings
        /// </summary>
        public ConsoleSettings()
        {
            this.Threshold = NotificationLevel.Error;
        }

        /// <inheritDoc/>
        public override INotifier ToNotifier()
        {
            return new ConsoleNotifier(this, Console.WriteLine);
        }

        /// <summary>
        /// Creates example instance with dummy data.
        /// </summary>
        public static NotifierSettings GetExample()
        {
            return new ConsoleSettings();
        }
    }
}
