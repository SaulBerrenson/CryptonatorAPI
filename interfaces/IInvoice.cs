using CryptonatorApi.enums;

namespace CryptonatorApi.interfaces
{
    public interface IInvoice
    {
        string invoice_id { get; set; }
        string item_name { get; set; }
        string order_id { get; set; }
        string item_description { get; set; }
        CurrencyType checkout_currency { get; set; }
        decimal invoice_amount { get; set; }
        CurrencyType invoice_currency { get; set; }
        string success_url { get; set; }
        string failed_url { get; set; }
        LangPayment language { get; set; }
        string UrlInvoice { get; set; }
    }
}