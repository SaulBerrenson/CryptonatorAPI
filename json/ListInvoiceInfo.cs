using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptonatorApi.json
{
    public class ListInvoiceInfo
    {
        [JsonProperty("invoice_count")]
        public int InvoiceCount;

        [JsonProperty("invoice_list")]
        public List<string> InvoiceList;
    }
}