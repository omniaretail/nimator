using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Notifiers;

namespace Nimator.Settings
{
    public class OpsGenieSettings : NotifierSettings
    {
        public string ApiKey { get; set; }

        public string TeamName { get; set; }

        public string HeartbeatName { get; set; }

        public override INotifier ToNotifier()
        {
            return new OpsGenieNotifier(this);
        }
    }
}
