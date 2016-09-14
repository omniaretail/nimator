using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nimator.Settings;
using Nimator.Util;

namespace Nimator.Notifiers
{
    internal class SlackNotifier : INotifier
    {
        private readonly SlackSettings settings;
        private DateTime dontAlertBefore;

        public SlackNotifier(SlackSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.Url)) throw new ArgumentException("settings.Url was not set", nameof(settings));

            this.settings = settings;
        }

        public void Notify(INimatorResult result)
        {
            if (settings.DebounceTimeInSecs > 0 && DateTime.Now < dontAlertBefore)
            {
                return;
            }

            if (result.Level >= settings.Threshold)
            {
                var message = new SlackMessage(result);
                
                if (settings.DebounceTimeInSecs > 0){
                    dontAlertBefore = DateTime.Now.AddSeconds(settings.DebounceTimeInSecs);
                    message.AddAttachment("Debouncing next notification until at least *" + dontAlertBefore.ToString("yyyy-MM-dd HH:mm:ss") + "*, even if more problems arise.");
                }
                    
                SimpleRestUtils.PostToRestApi(settings.Url, message);
            }
        }
    }
}
