using System;
using System.Threading.Tasks;
using Nimator.CouchDb.Models;

namespace Nimator.CouchDb
{
    public class MemoryCheck : ICheck
    {
        private readonly MemoryCheckSettings _settings;
        private readonly ICouchDbService _service;

        public MemoryCheck(MemoryCheckSettings settings, ICouchDbService service)
        {
            Guard.AgainstNull(nameof(settings), settings);
            Guard.AgainstNull(nameof(service), service);

            _settings = settings;
            _service = service;
        }

        public string ShortName => nameof(MemoryCheck);

        public async Task<ICheckResult> RunAsync()
        {
            NotificationLevel returnValue;
            var message = string.Empty;

            try
            {
                var nodeInformation = await _service.GetNodeInformation();
                returnValue = CheckForMemoryAvailability(nodeInformation);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                returnValue = NotificationLevel.Error;
            }


            return new CheckResult(this.ShortName, returnValue, message);
        }

        /// <summary>
        /// Checks for if there is enough memory on the server
        /// </summary>
        /// <param name="nodeInformation">Node data from the couchDb</param>
        /// <returns><see cref="NotificationLevel"/></returns>
        private NotificationLevel CheckForMemoryAvailability(NodeInformation nodeInformation)
        {
            var availableMemory = ((double)nodeInformation.storageTotals.ram.total / (double)nodeInformation.storageTotals.ram.used - 1) * 100;

            return availableMemory > _settings.MinimalMemoryPercentage
                ? NotificationLevel.Okay
                : NotificationLevel.Warning;
        }
    }
}
