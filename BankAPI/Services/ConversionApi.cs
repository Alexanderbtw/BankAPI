using BankAPI.Data;
using BankAPI.Data.Enums;
using BankAPI.Interfaces;

namespace BankAPI.Services
{
    public class ConversionApi : IConversionService
    {
        private HttpClient httpClient { get; set; }
        private string Key { get; set; }
        private readonly string Url = "http://data.fixer.io/api/latest?";

        public ConversionApi(string apikey, IHttpClientFactory factory) 
        {
            Key = apikey;
            httpClient = factory.CreateClient();
            httpClient.BaseAddress = new Uri(Url);
        }

        public decimal Convert(decimal money, Currency currencyFrom, Currency currencyTo)
        {
            var res = httpClient.GetFromJsonAsync(httpClient.BaseAddress + "access_key=" + Key, typeof(CurrencyResult)).Result as CurrencyResult;
            Dictionary<string, decimal>? currencies = res?.Rates;
            if (currencies != null)
                return Math.Round(currencies[currencyTo.ToString()] / currencies[currencyFrom.ToString()] * money, 2);
            return money;
        }

        public async Task<decimal> ConvertAsync(decimal money, Currency currencyFrom, Currency currencyTo)
        {
            var res = (await httpClient.GetFromJsonAsync(httpClient.BaseAddress + "access_key=" + Key, typeof(CurrencyResult))) as CurrencyResult;
            Dictionary<string, decimal>? currencies = res?.Rates;
            if (currencies != null)
                return Math.Round(currencies[currencyTo.ToString()] / currencies[currencyFrom.ToString()] * money, 2);
            return money;
        }
    }
}
