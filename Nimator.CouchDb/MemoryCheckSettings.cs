namespace Nimator.CouchDb
{
    /// <summary>
    /// Settings for checks that don't do anything besides returning a specific result after an (optional) delay.
    /// </summary>
    public class MemoryCheckSettings : ICheckSettings
    {
        private readonly string _credentials;
        private readonly string _connectionString;

        public int MinimalMemoryPercentage => 15;

        /// <summary>
        /// Constructs default settings.
        /// </summary>
        public MemoryCheckSettings(string connectionString, string credentials)
        {
            Guard.AgainstNull(nameof(connectionString), connectionString);
            Guard.AgainstNull(nameof(credentials), credentials);

            _connectionString = connectionString;
            _credentials = credentials;
        }

        /// <inheritDoc/>
        public ICheck ToCheck()
        {
            return new MemoryCheck(this, new CouchDbService(_connectionString, _credentials));
        }
    }
}
