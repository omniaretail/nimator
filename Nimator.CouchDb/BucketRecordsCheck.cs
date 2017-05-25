using System;
using System.Threading.Tasks;

namespace Nimator.CouchDb
{
    /// <summary>
    /// Example <see cref="ICheck"/> that does no actual check but always returns a
    /// certain result.
    /// </summary>
    public class BucketRecordsCheck : ICheck
    {
        private readonly BucketRecordsCheckSettings _settings;
        private readonly ICouchDbService _service;

        /// <summary>
        /// Constructs a check based on certain <see cref="BucketRecordsCheckSettings"/>.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="service"></param>
        public BucketRecordsCheck(BucketRecordsCheckSettings settings, ICouchDbService service)
        {
            Guard.AgainstNull(nameof(settings), settings);
            Guard.AgainstNull(nameof(service), service);

            _settings = settings;
            _service = service;
        }

        /// <inheritDoc/>
        public string ShortName => nameof(BucketRecordsCheck);

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
                var bucketInformation = await _service.GetBucketInformation(_settings.Bucket);
                returnValue = CheckForAvailableRecords(bucketInformation);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                returnValue = NotificationLevel.Error;
            }


            return new CheckResult(this.ShortName, returnValue, message);
        }

        private NotificationLevel CheckForAvailableRecords(BucketInformation bucketInformation)
        {
            return bucketInformation.basicStats.itemCount < _settings.MaximumRecords ? NotificationLevel.Okay : NotificationLevel.Warning;
        }
    }
}
