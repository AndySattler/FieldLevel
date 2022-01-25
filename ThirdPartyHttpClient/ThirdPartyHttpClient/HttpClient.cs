using System;
using System.Collections.Generic;
using ThirdPartyHttpClient.DTO;
using System.Threading.Tasks;

namespace ThirdPartyHttpClient
{
    public interface IHttpClient
    {
        public Task<IEnumerable<DTO.Post>> GetPosts();
    }


    public class HttpClient : IHttpClient
    {
        private static System.Net.Http.HttpClient _httpClient;
        private readonly string _baseUrl = "https://jsonplaceholder.typicode.com";
        public HttpClient()
        {
            if (_httpClient == null)
                _httpClient = new System.Net.Http.HttpClient();
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            var results = await _httpClient.GetAsync($"{_baseUrl}/posts");
            var stringData = await results.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<DTO.Post>>(stringData);
        }
    }
}
