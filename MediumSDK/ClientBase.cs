using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediumSDK
{
    public abstract class ClientBase
    {
        private const string BasePath = "https://api.medium.com";
        private const string ApiVersion = "v1";

        protected string BaseUrl => $"{BasePath}/{ApiVersion}";

        protected async Task<T> SendRequestAsync<T>(HttpRequestMessage request)
        {
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Charset", "utf-8");

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content).ToObject<T>();
            }
        }

        protected async Task<T> SendRequestWithTokenAsync<T>(HttpRequestMessage request, string accessToken)
        {
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            return await SendRequestAsync<T>(request);
        }


        protected Uri BuildRequestUri(string requestUrl, Dictionary<string, string> paramSet)
        {
            var sb = new StringBuilder(requestUrl);
            sb.Append("?");
            var querys = paramSet
                .Select(p => $"{p.Key}={WebUtility.UrlEncode(p.Value)}")
                .Aggregate((c, n) => $"{c}&{n}");
            sb.Append(querys);
            return new Uri(sb.ToString());
        }
    }
}
