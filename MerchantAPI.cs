using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CryptonatorAPI.enums;
using CryptonatorAPI.exceptions;
using CryptonatorAPI.Extantions;
using CryptonatorAPI.interfaces;
using CryptonatorAPI.json;

namespace CryptonatorAPI
{
    public class MerchantAPI
    {
        #region Constructors
        public MerchantAPI(string _merchantId, string _secretHash)
        {
            merchant_id = _merchantId;
            secret_hash = _secretHash;
        }
        #endregion

        #region public
        public string merchant_id { get; private set; }
        #endregion
        #region private
        private string secret_hash;
        private const string ApiUrl = "https://api.cryptonator.com/api/merchant/v1/";
        #endregion

        #region Methods
        #region /startpayment
        /// <summary>
        /// Request to create new invoice
        /// </summary>
        /// <param name="itemName">Name of an item or service.</param>
        /// <param name="amount">Invoice amount</param>
        /// <param name="currency">Invoice currency</param>
        /// <param name="language">Language of the checkout page</param>
        /// <param name="order_id">Order ID for your accounting purposes</param>
        /// <param name="item_description">Description of an item or service.</param>
        /// <param name="success_url">An URL to which users will be redirected after a successful payment</param>
        /// <param name="failed_url">An URL to which users will be redirected after a cancelled or failed payment</param>
        /// <returns></returns>
        public async Task<IInvoice> StartPayment(string itemName,
            string amount = "0.0015",
            CurrencyType currency = CurrencyType.bitcoin,
            LangPayment language = LangPayment.ru,
            string order_id = "",
            string item_description = "",
            string success_url = "",
            string failed_url = "")
        {

            if(merchant_id.IsNullOrWhiteSpaces() || secret_hash.IsNullOrWhiteSpaces()) throw new ApiFailed($"{nameof(merchant_id)} or {nameof(secret_hash)} is empty or null");
            if(itemName.IsNullOrWhiteSpaces() || amount.IsNullOrWhiteSpaces()) throw new ApiFailed($"Required parameters is null or empty!");


            try
            {
                using (HttpClient requestClient = HttpCreator.Create())
                {
                   
                    string queryUrl = String.Empty;
                    #region PostData

                    List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("merchant_id", merchant_id),
                        new KeyValuePair<string, string>("item_name", itemName),
                    };

                    if (!order_id?.IsNullOrWhiteSpaces() == true)
                    {
                        requestData.Add(new KeyValuePair<string, string>(nameof(order_id), order_id));
                    }

                    if (!item_description?.IsNullOrWhiteSpaces() == true)
                    {
                        requestData.Add(new KeyValuePair<string, string>(nameof(item_description), item_description));
                    }


                    requestData.AddRange(new[]
                    {
                        new KeyValuePair<string, string>("invoice_amount", amount.ToString()),
                        new KeyValuePair<string, string>("invoice_currency", currency.ToString()),
                    });

                    if (!success_url?.IsNullOrWhiteSpaces() == true)
                    {
                        requestData.Add(new KeyValuePair<string, string>(nameof(success_url), success_url));
                    }

                    if (!failed_url?.IsNullOrWhiteSpaces() == true)
                    {
                        requestData.Add(new KeyValuePair<string, string>(nameof(failed_url), failed_url));
                    }

                    requestData.Add(new KeyValuePair<string, string>("language", language.ToString()));

                    queryUrl = await new FormUrlEncodedContent(requestData).ReadAsStringAsync();

                    #endregion

                    #region Request
                    var responseMessage = await requestClient.GetAsync(string.Concat(ApiUrl, "startpayment", "/?", queryUrl));

                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                        throw new ApiFailed("Api error response",
                            new ApiFailed(await responseMessage?.Content?.ReadAsStringAsync()));
                    #endregion

                    #region Parse result
                    var url = responseMessage.RequestMessage.RequestUri.AbsoluteUri;
                    var st = responseMessage.RequestMessage.RequestUri.AbsoluteUri.LastIndexOf('/');
                    var id = responseMessage.RequestMessage.RequestUri.AbsoluteUri.Substring(st+1, responseMessage.RequestMessage.RequestUri.AbsoluteUri.Length-st-1);
                    #endregion

                    #region Result
                    return new InvoiceTicket(itemName, Convert.ToDecimal(amount, new CultureInfo("en-US")), currency, currency, language)
                    {
                        order_id = order_id,
                        item_description = item_description,
                        success_url = success_url,
                        failed_url = failed_url,

                        UrlInvoice = url,
                        invoice_id = id,

                    };
                    #endregion
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion

        #region /getinvoice
        /// <summary>
        /// Request to return invoice status
        /// </summary>
        /// <param name="invoice_id">Invoice ID</param>
        /// <returns></returns>
        public async Task<InvoiceInfo> GetiInvoice(string invoice_id)
        {
          return await getinvoice(invoice_id);
        }
        /// <summary>
        /// Request to return invoice status
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public async Task<InvoiceInfo> GetiInvoice(IInvoice invoice)
        {
            return await getinvoice(invoice.invoice_id);
        }

        private async Task<InvoiceInfo> getinvoice(string invoice_id)
        {
            if (merchant_id.IsNullOrWhiteSpaces() || secret_hash.IsNullOrWhiteSpaces()) throw new ApiFailed($"{nameof(merchant_id)} or {nameof(secret_hash)} is empty or null");
            if (invoice_id.IsNullOrWhiteSpaces()) throw new ApiFailed($"ID parameters is null or empty!");


            try
            {
                using (HttpClient requestClient = HttpCreator.Create())
                {
                   #region PostData
                    var postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("merchant_id",merchant_id),
                        new KeyValuePair<string, string>("invoice_id",invoice_id),
                        new KeyValuePair<string, string>("secret_hash",secret_hash),
                    }.SignPostData());
                    #endregion

                    #region Request
                    var responseMessage = await requestClient.PostAsync(string.Concat(ApiUrl, "getinvoice"), postContent);

                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                        throw new ApiFailed("Api error response",
                            new ApiFailed(await responseMessage?.Content?.ReadAsStringAsync()));
                    #endregion

                    #region Result
                    return (await responseMessage.Content.ReadAsStringAsync())?.Deserialize<InvoiceInfo>();
                    #endregion
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region /listinvoices
        /// <summary>
        /// Request to return all invoices for the account
        /// </summary>
        /// <param name="invoice_status">
        /// Invoice status:
        /// paid
        /// unpaid
        /// cancelled
        /// mispaid
        /// </param>
        /// <param name="invoice_currency">
        ///blackcoin
        /// bitcoin
        /// dash
        /// dogecoin
        /// emercoin
        /// litecoin
        /// peercoin
        /// </param>
        /// <param name="checkout_currency">
        /// blackcoin
        /// bitcoin
        /// dash
        /// dogecoin
        /// emercoin
        /// litecoin
        /// peercoin</param>
        /// <returns></returns>
        public async Task<ListInvoiceInfo> Listinvoices(string invoice_status = "",string invoice_currency="",string checkout_currency="")
        {
            if (merchant_id.IsNullOrWhiteSpaces() || secret_hash.IsNullOrWhiteSpaces()) throw new ApiFailed($"{nameof(merchant_id)} or {nameof(secret_hash)} is empty or null");
            try
            {
                using (HttpClient requestClient = HttpCreator.Create())
                {
                    #region PostData
                    var postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("merchant_id",merchant_id),
                        new KeyValuePair<string, string>("invoice_status",invoice_status),
                        new KeyValuePair<string, string>("invoice_currency",invoice_currency),
                        new KeyValuePair<string, string>("checkout_currency",checkout_currency),
                        new KeyValuePair<string, string>("secret_hash",secret_hash),
                    }.SignPostData());
                    #endregion

                    #region Request
                    var responseMessage = await requestClient.PostAsync(string.Concat(ApiUrl, "listinvoices"), postContent);

                    if (responseMessage.StatusCode != HttpStatusCode.OK)
                        throw new ApiFailed("Api error response",
                            new ApiFailed(await responseMessage?.Content?.ReadAsStringAsync()));
                    #endregion

                    #region Result
                    return (await responseMessage.Content.ReadAsStringAsync())?.Deserialize<ListInvoiceInfo>();
                    #endregion
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region /createinvoice
        /// <summary>
        /// Request to create new invoice with preset checkout currency
        /// </summary>
        /// <param name="itemName">Name of an item or service.</param>
        /// <param name="amount">Invoice amount</param>
        /// <param name="currency">Invoice currency</param>
        /// <param name="language">Language of the checkout page</param>
        /// <param name="order_id">Order ID for your accounting purposes</param>
        /// <param name="item_description">Description of an item or service.</param>
        /// <param name="success_url">An URL to which users will be redirected after a successful payment</param>
        /// <param name="failed_url">An URL to which users will be redirected after a cancelled or failed payment</param>
        /// <returns></returns>
        public async Task<InvoiceWithSign> CreateInvoice(string itemName,
            string amount = "0.0015",
            CurrencyType currency = CurrencyType.bitcoin,
            LangPayment language = LangPayment.ru,
            string order_id = "",
            string item_description = "",
            string success_url = "",
            string failed_url = "")
        {
            if (merchant_id.IsNullOrWhiteSpaces() || secret_hash.IsNullOrWhiteSpaces()) throw new ApiFailed($"{nameof(merchant_id)} or {nameof(secret_hash)} is empty or null");
            try
            {
                using (HttpClient requestClient = HttpCreator.Create())
                {
                   

                    #region PostData
                    var postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("merchant_id",merchant_id),
                        new KeyValuePair<string, string>("item_name",itemName),
                        new KeyValuePair<string, string>("order_id",order_id),
                        new KeyValuePair<string, string>("item_description", item_description),
                        new KeyValuePair<string, string>("checkout_currency",currency.ToString()),
                        new KeyValuePair<string, string>("invoice_amount",amount),
                        new KeyValuePair<string, string>("invoice_currency",currency.ToString()),
                        new KeyValuePair<string, string>("success_url",success_url),
                        new KeyValuePair<string, string>("failed_url",failed_url),
                        new KeyValuePair<string, string>("language",language.ToString()),

                        new KeyValuePair<string, string>("secret_hash",secret_hash),
                    }.SignPostData());
                    #endregion

                    #region Request
                    var responseMessage = await requestClient.PostAsync(string.Concat(ApiUrl, "createinvoice"), postContent);

                    if (responseMessage.StatusCode != HttpStatusCode.Created)
                        throw new ApiFailed("Api error response",
                            new ApiFailed(await responseMessage?.Content?.ReadAsStringAsync()));
                    #endregion

                    #region Result
                    return (await responseMessage.Content.ReadAsStringAsync())?.Deserialize<InvoiceWithSign>();
                    #endregion
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        #endregion

    }
}