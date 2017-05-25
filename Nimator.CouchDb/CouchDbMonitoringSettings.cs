using System.Configuration;

namespace Nimator.CouchDb
{
    public class CouchDbMonitoringSettings
    {
        public string ConnectionString { get; set; }
        public string Bucket { get; set; }

        public string Credentials
        {
            get
            {
                var username = ConfigurationManager.AppSettings["CouchDbUsername"];
                var password = ConfigurationManager.AppSettings["CouchDbPassword"];

                return $"{username}:{password}";
            }
        }

        //public IEnumerable<string> MonitoringTypes { get; set; }
    }
}
