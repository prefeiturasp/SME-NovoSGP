using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace SME.SGP.TesteIntegracao.Setup
{
    public static class TestsExtensions
    {
        public static void SetJsonMediaType(this HttpClient client)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public static void SetAuthorizeBearer(this HttpClient client, string token)
        {
            client.SetJsonMediaType();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return httpClient.PostAsync(url, content);
        }
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data, new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PutAsync(url, content);
        }
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content) where T : class
        {
            var dataAsString = await content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(dataAsString))
                return null;

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, IgnoreNullValues = true });
        }

        public static void EnsureSuccessStatusCodeOrResponseException(this HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                throw new System.Exception($"({(int)response.StatusCode}-{response.StatusCode}) {result}");
            }
        }

        public static string Truncate(this string s, int length)
        {
            if (s.Length > length) return s.Substring(0, length);
            return s;
        }
    }
}
