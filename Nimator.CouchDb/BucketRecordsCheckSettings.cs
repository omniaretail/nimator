namespace Nimator.CouchDb
{
    /// <summary>
    /// Settings for checks that don't do anything besides returning a specific result after an (optional) delay.
    /// </summary>
    public class BucketRecordsCheckSettings : ICheckSettings
    {
        private readonly string _credentials;
        private readonly string _connectionString;
        public string Bucket { get; }
        public int MaximumRecords => 100000;

        /// <summary>
        /// Constructs default settings.
        /// </summary>
        public BucketRecordsCheckSettings(string connectionString, string credentials, string bucket)
        {
            Guard.AgainstNull(nameof(connectionString), connectionString);
            Guard.AgainstNull(nameof(credentials), credentials);
            Guard.AgainstNull(nameof(bucket), bucket);

            _connectionString = connectionString;
            _credentials = credentials;
            Bucket = bucket;
        }

        /// <inheritDoc/>
        public ICheck ToCheck()
        {
            return new BucketRecordsCheck(this, new CouchDbService(_connectionString, _credentials));
        }
    }
}
