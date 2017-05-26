namespace Nimator.CouchDb
{
    public class MemoryCheckSettings : ICheckSettings
    {
        public string ConnectionString { get; set; }
        public string Credentials { get; set; }

        /// <summary>
        /// Minimal available memory on the system
        /// </summary>
        public int MinimalMemoryPercentage => 15;

        public MemoryCheckSettings(string connectionString, string credentials)
        {
            Guard.AgainstNull(nameof(connectionString), connectionString);
            Guard.AgainstNull(nameof(credentials), credentials);

            ConnectionString = connectionString;
            Credentials = credentials;
        }

        public ICheck ToCheck()
        {
            return new MemoryCheck(this, new CouchDbService(ConnectionString, Credentials));
        }
    }
}
