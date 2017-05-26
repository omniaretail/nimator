using System.Threading.Tasks;
using Nimator.CouchDb.Models;

namespace Nimator.CouchDb
{
    public interface ICouchDbService
    {
        /// <summary>
        /// Getting all the server data for a specified bucket
        /// </summary>
        /// <param name="bucket">Bucket name to get data from</param>
        /// <returns></returns>
        Task<BucketInformation> GetBucketInformation(string bucket);

        /// <summary>
        /// Getting all the server data for the whole node
        /// </summary>
        /// <returns></returns>
        Task<NodeInformation> GetNodeInformation();
    }
}