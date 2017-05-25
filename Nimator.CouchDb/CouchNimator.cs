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
        private readonly INimator _nimator;
        private CouchNimator(INimator nimator)
        {
            Guard.AgainstNull(nameof(nimator), nimator);

            _nimator = nimator;
        }

        public void TickSafe(ILog logger)
        {
            _nimator.TickSafe(logger);
        }

        public static INimator FromSettings(ILog logger, CouchDbMonitoringSettings monitorSettings, string json = "{}")
        {
            Guard.AgainstNull(nameof(logger), logger);
            Guard.AgainstNull(nameof(monitorSettings), monitorSettings);

            var settings = NimatorSettings.FromJson(json);
            var layers = settings.Layers.ToList();

            layers.Add(new LayerSettings
            {
                Name = "CouchDb",
                Checks = new ICheckSettings[]
                {
                    new MemoryCheckSettings(monitorSettings.ConnectionString, monitorSettings.Credentials),
                    new BucketRecordsCheckSettings(monitorSettings.ConnectionString, monitorSettings.Bucket, monitorSettings.Credentials)
                }
            });

            settings.Layers = layers.ToArray();

            return new CouchNimator(Nimator.FromSettings(logger, settings.ToJson()));
        } 
    }
}
