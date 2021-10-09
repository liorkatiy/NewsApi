using NewsApi.Entities.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsApi.Logic
{
    public class NewsHttpClient : HttpClient, INewsHttpClient
    {
        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> queryParams = null)
        {
            var paramsStr = queryParams is null ? null : string.Join('&', queryParams.Select(q => $"{q.Key}={q.Value}"));
            var requestUri = paramsStr is null ? url : $"{url}?{paramsStr}";
            var res = await GetAsync(requestUri);
            var contentResponse = await res.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(contentResponse);
            var response = responseJson.ToObject<T>();
            return response;
        }
    }
}