using CryptonatorAPI.enums;

namespace CryptonatorAPI
{
    /// <summary>
    /// Base Invoice Tiket
    /// </summary>
    public class InvoiceTicket : CryptonatorAPI.interfaces.IInvoice
    {
        #region Constructors
        public InvoiceTicket(string itemName, decimal invoiceAmount, CurrencyType checkoutCurrency = CurrencyType.bitcoin, CurrencyType invoiceCurrency = CurrencyType.bitcoin, LangPayment _lang = LangPayment.ru)
        {
            item_name = itemName;
            checkout_currency = checkoutCurrency;
            invoice_amount = invoiceAmount;
            invoice_currency = invoiceCurrency;
            language = _lang;
        }

        public InvoiceTicket(string invoiceId)
        {
            invoice_id = invoiceId;
        }

        public InvoiceTicket()
        {
        }
        #endregion

        public string invoice_id { get; set; }
        public string item_name { get; set; }
        public string order_id { get; set; }
        public string item_description { get; set; }
        public CurrencyType checkout_currency { get; set; }
        public decimal invoice_amount { get; set; }
        public CurrencyType invoice_currency { get; set; }
        public string success_url { get; set; }
        public string failed_url { get; set; }
        public LangPayment language { get; set; }
        public string UrlInvoice { get; set; }
    }
}