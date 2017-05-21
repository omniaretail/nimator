using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Nimator.Settings;

namespace Nimator.CouchDb
{
    public class CouchNimator : INimator
    {
        public CouchNimator()
        {

        }

        public void TickSafe(ILog logger)
        {
            throw new NotImplementedException();
        }

        public static INimator FromSettings(ILog logger, string json, MonitoringSettings monitorSettings)
        {
            return Nimator.FromSettings(logger, json);
        }
    }
}
