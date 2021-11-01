using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Repository.Rest
{
    /// <summary>
    /// Contains methods for interacting with the bridges backend using REST. 
    /// </summary>
    public class RestHistoryRepository : IHistoryRepository
    {
        private readonly HttpHelper _http;

        public RestHistoryRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public async Task<IEnumerable<History>> GetAsync() =>
            await _http.GetAsync<IEnumerable<History>>("history");

        public async Task<IEnumerable<History>> GetAsync(string search) =>
            await _http.GetAsync<IEnumerable<History>>($"history/search?value={search}");

        public async Task<History> GetAsync(Guid id) =>
            await _http.GetAsync<History>($"history/{id}");

        public async Task<History> UpsertAsync(History history) =>
            await _http.PostAsync<History, History>("history", history);

        public async Task DeleteAsync(Guid bridgeId) =>
            await _http.DeleteAsync("history", bridgeId);
    }
}
