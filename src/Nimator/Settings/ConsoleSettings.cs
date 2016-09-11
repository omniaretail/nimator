using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
