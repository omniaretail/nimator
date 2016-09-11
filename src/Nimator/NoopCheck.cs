using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public class NoopCheck : ICheck
    {
        private readonly NoopCheckSettings settings;

        public NoopCheck(NoopCheckSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            this.settings = settings;
        }

        public string ShortName
        {
            get { return "Always" + settings.LevelToSimulate.ToString(); }
        }

        public async Task<ICheckResult> RunAsync()
        {
            await Task.Delay(settings.DelayResultInMs);

            if (settings.LevelToSimulate == NotificationLevel.Critical)
            {
                throw new Exception("Simulating bug in Check, throwing exception.");
            }

            return new CheckResult(this.ShortName, settings.LevelToSimulate);
        }
    }
}
