using System;
using System.Threading.Tasks;
using CryptonatorAPI.enums;
using CryptonatorAPI.Extantions;
using CryptonatorApi.json;

namespace CryptonatorAPI
{
    public class InfoAPI
    {
        #region privateMethods
        private string urlBase(UrlTickerType type)
        {
            switch (type)
            {
                case UrlTickerType.SimpleTicker:
                    return "https://api.cryptonator.com/api/ticker/";
                case UrlTickerType.CompleteTicker:
                    return "https://api.cryptonator.com/api/full/";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        #endregion


        public async Task<TickerResponse> Get(UrlTickerType tickerType = UrlTickerType.SimpleTicker, string direction = "btc-usd")
        {
            try
            {
                using (var client = HttpCreator.Create())
                {
                    string url = string.Concat(urlBase(tickerType), direction);

                    var jsonString= await client.GetStringAsync(url);

                    return jsonString.Deserialize<TickerResponse>();

                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }


    }
}