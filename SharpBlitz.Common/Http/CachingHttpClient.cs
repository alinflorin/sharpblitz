using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SharpBlitz.Common.Http
{
    public class CachingHttpClient
    {
        public static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        public static HttpClient _http = new HttpClient();

        public async Task<byte[]> GetStreamAsync(string url)
        {
            if (_cache.ContainsKey(url))
            {
                return (byte[])_cache[url];
            }
            var ba = await _http.GetByteArrayAsync(url);
            _cache[url] = ba;
            return ba;
        }
    }
}