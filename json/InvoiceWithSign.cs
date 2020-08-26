using CryptonatorAPI.interfaces;
using Newtonsoft.Json;

namespace CryptonatorAPI.json
{
    public class InvoiceWithSign : IInvoice
    {
        [JsonProperty("invoice_id")]
        public string invoice_id { get; set; }

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

        public string URL => string.Concat("https://rf.cryptonator.com/merchant/invoice/", invoice_id);
    }
}