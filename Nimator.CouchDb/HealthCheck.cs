using System;
using System.Threading.Tasks;

namespace Nimator.CouchDb
{
    /// <summary>
    /// Example <see cref="ICheck"/> that does no actual check but always returns a
    /// certain result.
    /// </summary>
    public class HealthCheck : ICheck
    {
        private readonly HealthCheckSettings settings;

        /// <summary>
        /// Constructs a check based on certain <see cref="HealthCheckSettings"/>.
        /// </summary>
        /// <param name="settings"></param>
        public HealthCheck(HealthCheckSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritDoc/>
        public string ShortName => nameof(HealthCheck);

        /// <summary>
        /// Will return a task that promises a <see cref="ICheckResult"/>, after a possible Delay.
        /// </summary>
        /// <returns></returns>
        public async Task<ICheckResult> RunAsync()
        {
            var returnValue = NotificationLevel.Okay;
            await Task.Delay(1);

            if (returnValue == NotificationLevel.Critical)
            {
                throw new Exception("Simulating bug in Check, throwing exception.");
            }

            return new CheckResult(this.ShortName, returnValue);
        }
    }
}
