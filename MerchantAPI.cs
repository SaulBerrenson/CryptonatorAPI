using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CryptonatorApi.enums;
using CryptonatorApi.exceptions;
using CryptonatorApi.Extantions;
using CryptonatorApi.interfaces;

namespace CryptonatorApi
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

        public string merchant_id { get; private set; }

        #region private
        private string secret_hash;
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36";
        private const string ApiUrl = "https://api.cryptonator.com/api/merchant/v1/";
        #endregion



        #region /startpayment
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
                using (HttpClient requestClient = new HttpClient())
                {
                    #region Headers

                    requestClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                    #endregion
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
        public async void GetiInvoice(string invoice_id)
        {
            getinvoice(invoice_id);
        }
        public async void GetiInvoice(IInvoice invoice)
        {
            getinvoice(invoice.invoice_id);
        }

        private async void getinvoice(string invoice_id)
        {
            if (merchant_id.IsNullOrWhiteSpaces() || secret_hash.IsNullOrWhiteSpaces()) throw new ApiFailed($"{nameof(merchant_id)} or {nameof(secret_hash)} is empty or null");
            if (invoice_id.IsNullOrWhiteSpaces()) throw new ApiFailed($"ID parameters is null or empty!");


            try
            {
                using (HttpClient requestClient = new HttpClient())
                {
                    #region Headers

                    requestClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                    #endregion
                   
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

                    var str = await responseMessage.Content.ReadAsStringAsync();
                   

                    #region Result
                    return;
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



    }
}