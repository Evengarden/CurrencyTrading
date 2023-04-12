using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

        public async Task<ICollection<CurrencyDTO>> test()
        {
            var client = new RestClient("http://www.cbr.ru/scripts/XML_daily.asp");

            var request = new RestRequest();

            var response = await client.GetAsync(request);
            Console.WriteLine($"CONTENT {response.Content}");
            var xElement = XElement.Parse(response.Content);

            var currencies = DtoConvert.XmlToCurrencyDTO(xElement);

            return currencies;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await SendRequestToCB();
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
                    code = currency.CurrencyCode.ToString(),
                    nominal = currency.CurrencyNominal.ToString(),
                    price = currency.CurrencyPrice.ToString()
                });
                await _cache.SetStringAsync(currency.CurrencyCode, currencyJson, options);
                currencyCodes.Add(currency.CurrencyCode.ToString());
            }
            await _cache.SetStringAsync("codes", string.Join(",", currencyCodes));
        }
    }
}
