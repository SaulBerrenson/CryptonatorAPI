﻿using Newtonsoft.Json;

namespace CryptonatorAPI.json
{
    public class InvoiceInfo
    {
       
       [JsonProperty("order_id")]
       public string OrderId;

       [JsonProperty("amount")]
       public string Amount;

       [JsonProperty("currency")]
       public string Currency;

       [JsonProperty("status")]
       public string Status;
       
    }
}