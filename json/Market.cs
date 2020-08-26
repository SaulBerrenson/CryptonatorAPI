using Newtonsoft.Json;

namespace CryptonatorApi.json
{
    public class MarketResponse
    {
        [JsonProperty("market")]
        public string Market;

        [JsonProperty("price")]
        public string Price;

        [JsonProperty("volume")]
        public string Volume;
    }
}