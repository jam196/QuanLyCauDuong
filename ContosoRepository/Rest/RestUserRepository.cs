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
    public class RestUserRepository : IUserRepository
    {
        private readonly HttpHelper _http;

        public RestUserRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public async Task<IEnumerable<User>> GetAsync() =>
            await _http.GetAsync<IEnumerable<User>>("user");

        public async Task<IEnumerable<User>> GetAsync(string search) =>
            await _http.GetAsync<IEnumerable<User>>($"bridge/search?value={search}");

        public async Task<User> GetAsync(Guid id) =>
            await _http.GetAsync<User>($"bridge/{id}");
        public async Task<User> GetByEmailAsync(String email) =>
            await _http.GetAsync<User>($"bridge/{email}");

        public async Task<User> UpsertAsync(User bridge) =>
            await _http.PostAsync<User, User>("User", bridge);

        public async Task DeleteAsync(Guid bridgeId) =>
            await _http.DeleteAsync("User", bridgeId);
    }
}
