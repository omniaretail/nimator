namespace Nimator.CouchDb
{
    public class BucketRecordsCheckSettings : ICheckSettings
    {
        public string ConnectionString { get; set; }
        public string Credentials { get; set; }
        public string Bucket { get; set; }

        /// <summary>
        /// The maximum allowed records within specified bucket
        /// </summary>
        public int MaximumRecords => 100000;

        /// <summary>
        /// Constructs default settings.
        /// </summary>
        public BucketRecordsCheckSettings(string connectionString, string credentials, string bucket)
        {
            Guard.AgainstNull(nameof(connectionString), connectionString);
            Guard.AgainstNull(nameof(credentials), credentials);
            Guard.AgainstNull(nameof(bucket), bucket);

            ConnectionString = connectionString;
            Credentials = credentials;
            Bucket = bucket;
        }

        /// <inheritDoc/>
        public ICheck ToCheck()
        {
            return new BucketRecordsCheck(this, new CouchDbService(ConnectionString, Credentials));
        }
    }
}
