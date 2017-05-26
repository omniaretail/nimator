using System;
using System.Threading.Tasks;
using Nimator.CouchDb.Models;

namespace Nimator.CouchDb
{
    public class BucketRecordsCheck : ICheck
    {
        private readonly BucketRecordsCheckSettings _settings;
        private readonly ICouchDbService _service;

        public BucketRecordsCheck(BucketRecordsCheckSettings settings, ICouchDbService service)
        {
            Guard.AgainstNull(nameof(settings), settings);
            Guard.AgainstNull(nameof(service), service);

            _settings = settings;
            _service = service;
        }

        public string ShortName => nameof(BucketRecordsCheck);

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

        /// <summary>
        /// Checks for if there are not too many records in a specified bucket
        /// </summary>
        /// <param name="bucketInformation">Bucket data from the couchDb</param>
        /// <returns><see cref="NotificationLevel"/></returns>
        private NotificationLevel CheckForAvailableRecords(BucketInformation bucketInformation)
        {
            return bucketInformation.basicStats.itemCount < _settings.MaximumRecords ? NotificationLevel.Okay : NotificationLevel.Warning;
        }
    }
}
