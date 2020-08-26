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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickerType">
        /// Simple ticker - Returns actual volume-weighted price, total 24h volume and the price change.
        /// Complete ticker - Returns actual volume-weighted price, total 24h volume, rate change as well as prices and volumes across all connected exchanges. 
        /// </param>
        /// <param name="direction">Default request for BTC-USD</param>
        /// <returns></returns>
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