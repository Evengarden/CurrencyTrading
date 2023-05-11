using CurrencyTradig.CbClient.Interfaces;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.DAL.Interfaces;
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
  
    public class CurrencyService : ICurrencyService, IJob
    {
        public readonly ICurrencyRepository _currencyRepository;
        public readonly ICbApiClient _cb;
        public CurrencyService(ICurrencyRepository currencyRepository, ICbApiClient cb)
        {
            _currencyRepository = currencyRepository;
            _cb = cb;
        }
        public async Task<ICollection<CurrencyDTO>> GetCurrency()
        {
            var codes = await _currencyRepository.GetCurrencyCodes();
            var codesArr = codes.Split(",");
            List<CurrencyDTO> currencies = new List<CurrencyDTO>();
            foreach (var code in codesArr)
            {
                var currency = await _currencyRepository.GetCurrency(code);
                var currencyDeserialize = JsonConvert.DeserializeObject<CurrencyDTO>(currency);
                currencies.Add(currencyDeserialize);
            }
            return currencies;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await CacheCurrency();
            }
            catch (Exception ex)
            {
                await Task.Delay(300000);
                JobExecutionException qe = new JobExecutionException(ex);
                qe.RefireImmediately = true;
                throw qe;
            }
        }

        private async Task CacheCurrency()
        {
            var currencies = await _cb.sendRequestToCb();
            List<string> currencyCodes = new List<string>();
            foreach (var currency in currencies)
            {
                var currencyJson = JsonConvert.SerializeObject(new
                {
                    CurrencyCode = currency.CurrencyCode.ToString(),
                    CurrencyNominal = currency.CurrencyNominal.ToString(),
                    CurrencyPrice = currency.CurrencyPrice.ToString()
                });
                await _currencyRepository.SetCurrency(currency.CurrencyCode, currencyJson);
                currencyCodes.Add(currency.CurrencyCode.ToString());
            }
            await _currencyRepository.SetCurrencyCodes(currencyCodes);
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
            var currencyFromRedis = await _currencyRepository.GetCurrency(currency);
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
