using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsApi.Entities.Interfaces
{
    public interface INewsHttpClient
    {
        Task<T> GetAsync<T>(string url, Dictionary<string, string> queryParams = null);
    }
}