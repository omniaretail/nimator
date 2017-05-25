using System;
using System.Threading.Tasks;

namespace Nimator.CouchDb
{
    /// <summary>
    /// Example <see cref="ICheck"/> that does no actual check but always returns a
    /// certain result.
    /// </summary>
    public class MemoryCheck : ICheck
    {
        private readonly MemoryCheckSettings _settings;
        private readonly ICouchDbService _service;

        /// <summary>
        /// Constructs a check based on certain <see cref="MemoryCheckSettings"/>.
        /// </summary>
        /// <param name="settings"></param>
        public MemoryCheck(MemoryCheckSettings settings, ICouchDbService service)
        {
            Guard.AgainstNull(nameof(settings), settings);
            Guard.AgainstNull(nameof(service), service);

            this._settings = settings;
            _service = service;
        }

        /// <inheritDoc/>
        public string ShortName => nameof(MemoryCheck);

        /// <summary>
        /// Will return a task that promises a <see cref="ICheckResult"/>, after a possible Delay.
        /// </summary>
        /// <returns></returns>
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

        private NotificationLevel CheckForMemoryAvailability(NodeInformation nodeInformation)
        {
            var availableMemory = (1 - (double)nodeInformation.storageTotals.ram.used / (double)nodeInformation.storageTotals.ram.total) * 100;

            return availableMemory > _settings.MinimalMemoryPercentage
                ? NotificationLevel.Okay
                : NotificationLevel.Warning;
        }
    }
}
