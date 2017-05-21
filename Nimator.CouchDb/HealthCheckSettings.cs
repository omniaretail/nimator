namespace Nimator.CouchDb
{
    /// <summary>
    /// Settings for checks that don't do anything besides returning a specific result after an (optional) delay.
    /// </summary>
    public class HealthCheckSettings : ICheckSettings
    {
        /// <summary>
        /// Constructs default settings.
        /// </summary>
        public HealthCheckSettings()
        {
        }

        /// <inheritDoc/>
        public ICheck ToCheck()
        {
            return new HealthCheck(this);
        }
    }
}
