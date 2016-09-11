using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Notifiers;

namespace Nimator.Settings
{
    public class SlackSettings : NotifierSettings
    {
        public string Url { get; set; }

        public int DebounceTimeInSecs { get; set; }

        public override INotifier ToNotifier()
        {
            return new SlackNotifier(this);
        }
    }
}
