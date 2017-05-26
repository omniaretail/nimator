using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Nimator.Settings;

namespace Nimator.CouchDb
{
    /// <summary>
    /// Wraps the Nimator class to add extra checks for a couchDb
    /// </summary>
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

        /// <summary>
        /// Adds the memory and bucket records check to the CouchDb layer based on the monitoring settings provided.
        /// </summary>
        /// <param name="logger">Logger for logging checks</param>
        /// <param name="monitorSettings">Settings for couchDb checks</param>
        /// <param name="jsonSettings">Settings for adding other notifiers, layers and checks</param>
        /// <returns></returns>
        public static INimator FromSettings(ILog logger, CouchDbMonitoringSettings monitorSettings, string jsonSettings = "{}")
        {
            Guard.AgainstNull(nameof(logger), logger);
            Guard.AgainstNull(nameof(monitorSettings), monitorSettings);

            var settings = NimatorSettings.FromJson(jsonSettings);
            var layers = settings.Layers.ToList();

            layers.Add(new LayerSettings
            {
                Name = "CouchDb",
                Checks = new ICheckSettings[]
                {
                    new MemoryCheckSettings(monitorSettings.ConnectionString, monitorSettings.Credentials),
                    new BucketRecordsCheckSettings(monitorSettings.ConnectionString, monitorSettings.Credentials, monitorSettings.Bucket)
                }
            });

            settings.Layers = layers.ToArray();

            return new CouchNimator(Nimator.FromSettings(logger, settings.ToJson()));
        } 
    }
}
