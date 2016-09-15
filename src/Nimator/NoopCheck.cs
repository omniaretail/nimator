using System;
using System.Threading.Tasks;

namespace Nimator
{
    public class NoopCheck : ICheck
    {
        private readonly NoopCheckSettings settings;

        public NoopCheck(NoopCheckSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            this.settings = settings;
        }

        public string ShortName => "Always" + settings.LevelToSimulate;

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
