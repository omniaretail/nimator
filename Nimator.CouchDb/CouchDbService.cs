using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Nimator.CouchDb.Models;

namespace Nimator.CouchDb
{
    public class CouchDbService : ICouchDbService
    {
        private readonly string _connectionString;
        private readonly HttpClient _httpClient = new HttpClient();

        public CouchDbService(string connectionString, string credentials)
        {
            Guard.AgainstNull(nameof(connectionString), connectionString);
            Guard.AgainstNull(nameof(credentials), credentials);

            _connectionString = connectionString;

            var byteArray = Encoding.ASCII.GetBytes(credentials);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<BucketInformation> GetBucketInformation(string bucket)
        {
            var response = await _httpClient.GetAsync(_connectionString + "/pools/default/buckets/" + bucket);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<BucketInformation>(content);
        }

        public async Task<NodeInformation> GetNodeInformation()
        {
            var response = await _httpClient.GetAsync(_connectionString + "/pools/default");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<NodeInformation>(content);
        }
    }
}
