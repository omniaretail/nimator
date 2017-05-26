using System.Configuration;

namespace Nimator.CouchDb
{
    /// TODO: Make more bucket available for monitoring and give the option to set monitoring parameters
    public class CouchDbMonitoringSettings
    {
        /// <summary>
        /// The ConnectionStering to the CouchDb
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The Bucket you want to monitor
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// The Basic authenication Credentials that you need to acces the CouchDbApi 
        /// </summary>
        public string Credentials { get; set; }
    }
}
