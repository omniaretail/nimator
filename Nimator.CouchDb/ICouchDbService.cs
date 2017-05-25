using System.Threading.Tasks;

namespace Nimator.CouchDb
{
    public interface ICouchDbService
    {
        Task<BucketInformation> GetBucketInformation(string bucket);
        Task<NodeInformation> GetNodeInformation();
    }
}