using Newtonsoft.Json;

namespace CryptonatorApi.Extantions
{
    internal static class JsonExtantions
    {
        public static T Deserialize<T>(this string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
    }
}