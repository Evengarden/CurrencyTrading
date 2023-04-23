using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Quartz;
using RestSharp;
using System.Xml.Linq;

namespace CurrencyTrading.services.Services
{
  
    public class IntegrationService : IIntegrationService, IJob
    {
        public readonly IDistributedCache _cache;
        public IntegrationService(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
        }
        public async Task<ICollection<CurrencyDTO>> GetCurrencyFromRedis()
        {
            var codes = await _cache.GetStringAsync("codes");
            var codesArr = codes.Split(",");
            List<CurrencyDTO> currencies = new List<CurrencyDTO>();
            foreach (var code in codesArr)
            {
                var currency = await _cache.GetStringAsync(code);
                var currencyDeserialize = JsonConvert.DeserializeObject<CurrencyDTO>(currency);
                currencies.Add(currencyDeserialize);
            }
            return currencies;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await SendRequestToCB();
            }
            catch (Exception ex)
            {
                await Task.Delay(300000);
                JobExecutionException qe = new JobExecutionException(ex);
                qe.RefireImmediately = true;
                throw qe;
            }
        }

        private async Task SendRequestToCB()
        {
            var client = new RestClient("http://www.cbr.ru/scripts/XML_daily.asp");

            var request = new RestRequest();

            var response = await client.GetAsync(request);
            var xElement = XElement.Parse(response.Content);

            var currencies = DtoConvert.XmlToCurrencyDTO(xElement);
            List<string> currencyCodes = new List<string>();
            var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(24))
                        .SetSlidingExpiration(TimeSpan.FromHours(24));
            foreach (var currency in currencies)
            {
                var currencyJson = JsonConvert.SerializeObject(new
                {
                    CurrencyCode = currency.CurrencyCode.ToString(),
                    CurrencyNominal = currency.CurrencyNominal.ToString(),
                    CurrencyPrice = currency.CurrencyPrice.ToString()
                });
                await _cache.SetStringAsync(currency.CurrencyCode, currencyJson, options);
                currencyCodes.Add(currency.CurrencyCode.ToString());
            }
            await _cache.SetStringAsync("codes", string.Join(",", currencyCodes));
        }
        public async Task<decimal> CalculateLotPrice(string currency, decimal currencyAmount)
        {
            var currencyFromRedis = await CheckCurrencyExist(currency);
            var currencyDeserialize = JsonConvert.DeserializeObject<CurrencyDTO>(currencyFromRedis);
            decimal calculatedPrice = (currencyDeserialize.CurrencyPrice / currencyDeserialize.CurrencyNominal) * currencyAmount;
            return calculatedPrice;
        }

        public async Task<string?> CheckCurrencyExist(string currency)
        {
            var currencyFromRedis = await _cache.GetStringAsync(currency);
            if(currencyFromRedis == null)
            {
                throw new CurrencyNotFound 
                {
                    Currency = currency,
                };
            }
            return currencyFromRedis;
        }
    }
}
