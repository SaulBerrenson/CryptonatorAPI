using CryptonatorApi.interfaces;
using Newtonsoft.Json;

namespace CryptonatorApi.json
{
    public class InvoiceWithSign : IInvoice
    {
        [JsonProperty("invoice_id")]
        public string InvoiceId;

        [JsonProperty("checkout_address")]
        public string CheckoutAddress;

        [JsonProperty("checkout_amount")]
        public double CheckoutAmount;

        [JsonProperty("checkout_currency")]
        public string CheckoutCurrency;

        [JsonProperty("invoice_created")]
        public int InvoiceCreated;

        [JsonProperty("invoice_expires")]
        public int InvoiceExpires;

        [JsonProperty("secret_hash")]
        public string SecretHash;

        public string URL => string.Concat("https://rf.cryptonator.com/merchant/invoice/", InvoiceId);
    }
}