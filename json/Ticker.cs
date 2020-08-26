using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptonatorApi.json
{
    public class Ticker
    {
        [JsonProperty("base")]
        public string Base;

        [JsonProperty("target")]
        public string Target;

        [JsonProperty("price")]
        public string Price;

        [JsonProperty("volume")]
        public string Volume;

        [JsonProperty("change")]
        public string Change;

        [JsonProperty("markets")]
        public List<MarketResponse> Markets;
    }
}