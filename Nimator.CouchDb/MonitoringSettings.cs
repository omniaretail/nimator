using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.CouchDb
{
    public class MonitoringSettings
    {
        public string ConnectionString { get; set; }
        public IEnumerable<string> Buckets { get; set; }
        //public IEnumerable<string> MonitoringTypes { get; set; }
    }
}
