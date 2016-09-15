using System;
using System.Threading.Tasks;

namespace Nimator
{
    /// <summary>
    /// Example <see cref="ICheck"/> that does no actual check but always returns a
    /// certain result.
    /// </summary>
    public class NoopCheck : ICheck
    {
        private readonly NoopCheckSettings settings;

        /// <summary>
        /// Constructs a check based on certain <see cref="NoopCheckSettings"/>.
        /// </summary>
        /// <param name="settings"></param>
        public NoopCheck(NoopCheckSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            this.settings = settings;
        }

        /// <inheritDoc/>
        public string ShortName => "Always" + settings.LevelToSimulate;

        /// <summary>
        /// Will return a task that promises a <see cref="ICheckResult"/>, after a possible Delay.
        /// </summary>
        /// <returns></returns>
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
