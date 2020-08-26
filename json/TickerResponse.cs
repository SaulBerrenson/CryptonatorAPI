using Newtonsoft.Json;

namespace CryptonatorApi.json
{
    public class TickerResponse
    {
        [JsonProperty("ticker")]
        public Ticker Ticker;

        [JsonProperty("timestamp")]
        public int Timestamp;

        [JsonProperty("success")]
        public bool Success;

        [JsonProperty("error")]
        public string Error;
    }
}