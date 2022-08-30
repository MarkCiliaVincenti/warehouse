using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Vayosoft.Core.Utilities
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Deserialize object from json with JsonNet
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object</typeparam>
        /// <param name="json">json string</param>
        /// <returns>deserialized object</returns>
        public static T FromJson<T>(this string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return JsonSerializer.Deserialize<T>(json, options);
        }

        /// <summary>
        /// Serialize object to json with JsonNet
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>json string</returns>
        public static string ToJson(this object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Serialize object to json with JsonNet
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>json string</returns>
        public static StringContent ToJsonStringContent(this object obj)
        {
            return new StringContent(obj.ToJson(), Encoding.UTF8, "application/json");
        }
    }
}
