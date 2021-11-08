/*using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Repository.Rest
{
    /// <summary>
    /// Contains methods for interacting with the bridges backend using REST. 
    /// </summary>
    public class RestBridgeRepository : IBridgeRepository
    {
        private readonly HttpHelper _http;

        public RestBridgeRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public async Task<IEnumerable<Bridge>> GetAsync() =>
            await _http.GetAsync<IEnumerable<Bridge>>("bridge");

        public async Task<IEnumerable<Bridge>> GetAsync(string search) =>
            await _http.GetAsync<IEnumerable<Bridge>>($"bridge/search?value={search}");

        public async Task<Bridge> GetAsync(Guid id) =>
            await _http.GetAsync<Bridge>($"bridge/{id}");

        public async Task<Bridge> UpsertAsync(Bridge bridge) =>
            await _http.PostAsync<Bridge, Bridge>("bridge", bridge);

        public async Task DeleteAsync(Guid bridgeId) =>
            await _http.DeleteAsync("bridge", bridgeId);
    }
}
*/