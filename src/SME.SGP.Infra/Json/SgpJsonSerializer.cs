using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Json
{
    public static class SgpJsonSerializer
    {
        private static JsonSerializerOptions DefaultOptions(JsonSerializerOptions options = null)
        {
            var defaultOptions = options ?? new JsonSerializerOptions();
            defaultOptions.PropertyNameCaseInsensitive = true;

            defaultOptions.Converters.Add(new LongToStringConverter());

            return defaultOptions;
        }

        public static T Deserialize<T>(string json, JsonSerializerOptions options = null)
            => JsonSerializer.Deserialize<T>(json, DefaultOptions(options));
        public static async Task<T> DeserializeAsync<T>(string json, JsonSerializerOptions options = null)
            => await JsonSerializer.DeserializeAsync<T>(StringToStream(json), DefaultOptions(options));

        private static Stream StringToStream(string json)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            return new MemoryStream(byteArray);
        }

        public static string Serialize<TValue>(TValue value, JsonSerializerOptions options = null)
            => JsonSerializer.Serialize(value, DefaultOptions(options));

        public static async Task SerializeAsync<TValue>(Stream utf8Json, TValue value, JsonSerializerOptions options = null)
            => await JsonSerializer.SerializeAsync(utf8Json, value, DefaultOptions(options));
    }
}
