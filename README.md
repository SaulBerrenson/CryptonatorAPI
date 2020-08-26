# CryptonatorAPI

Lightweight library for work with with Info API and Merchant Account (API | MerchantAPI) (www.cryptonator.com)

### Usage

#### API Client
```
// Create Info Api client
InfoAPI api = new InfoAPI();

// Create Merchant Api client
MerchantAPI api = new MerchantAPI("your merchantID", "yourSecret");
```

### MerchantAPI
#### /startpayment
```
await api.StartPayment("My itemName", "0.0004");
//Response
InvoiceTicket
{
    invoice_id = "1324354354363ID",
    item_name = "My itemName",
    order_id = "",
    item_description = "",
    checkout_currency = "bitcoin",
    invoice_amount = 0.0004,
    invoice_currency = "bitcoin",
    success_url = "",
    failed_url = "",
    language = "ru",
    UrlInvoice = "https://rf.cryptonator.com/merchant/invoice/1324354354363ID"
}
```
#### /createinvoice
```
await api.CreateInvoice("Item 3546457")
//Response
InvoiceWithSign
{
    invoice_id = "id12345678",
    CheckoutAddress = "3535436address",
    CheckoutAmount = 0.0015,
    CheckoutCurrency = "bitcoin",
    InvoiceCreated = 1598433650,
    InvoiceExpires = 1598435450,
    SecretHash = "050d1c916e5c3a7fe51ddb4f89d94d688fb5a06f"
}
```
#### /getinvoice
```
var info= await api.GetiInvoice("invoiceID");
api.GetiInvoice(new InvoiceTicket("MyInvoiceTicket"))
api.GetiInvoice(new InvoiceWithSign() { invoice_id = "InvoiceID"})
//Response
InvoiceInfo
{
    OrderId = "3017203",
    Amount = "0.00150000",
    Currency = "bitcoin",
    Status = "unpaid"
}
```

#### /listinvoices
```
var info = await api.Listinvoices(invoice_status:"unpaid")
var info = await api.Listinvoices(invoice_status:"paid")

//Response
ListInvoiceInfo
{
    InvoiceCount = 2,
    InvoiceList = new List<string>
    {
        "4752470ee37d5da3572fed85cf1d401e",
        "d10fee867e11fbf7525d72690086eb5d",
        
    }
}
```
#### Info API (Example)
```
var data = await new InfoAPI().Get(UrlTickerType.SimpleTicker);
data = await new InfoAPI().Get(UrlTickerType.CompleteTicker);
data = await new InfoAPI().Get(UrlTickerType.SimpleTicker, "usd-btc");
data =  await new InfoAPI().Get(UrlTickerType.SimpleTicker, "usd-btc");
//Response
new TickerResponse
{
    Ticker = new Ticker
    {
        Base = "BTC",
        Target = "USD",
        Price = "11385.74817585",
        Volume = "104334.49053476",
        Change = "15.13313335",
        Markets = new List<MarketResponse>
        {
            new MarketResponse
            {
                Market = "Binance",
                Price = "11386.45000000",
                Volume = "69340.633018"
            },
            new MarketResponse
            {
                Market = "BitFinex",
                Price = "11388.00000000",
                Volume = "5783.95180452"
            },
       
        }
    },
    Timestamp = 1598436182,
    Success = true,
    Error = ""
}
```

