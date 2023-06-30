using CurrencyTrading.DAL.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CurrencyTrading.DAL.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public readonly IDistributedCache _cache;

        public CurrencyRepository(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
        }

        public async Task<string?> GetCurrency(string code)
        {
            return await _cache.GetStringAsync(code);
        }

        public async Task<string?> GetCurrencyCodes()
        {
            return await _cache.GetStringAsync("codes");
        }

        public async Task SetCurrency(string currencyCode, string currencyJson)
        {
            var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddHours(24))
                        .SetSlidingExpiration(TimeSpan.FromHours(24));
            await _cache.SetStringAsync(currencyCode, currencyJson, options);
        }

        public async Task SetCurrencyCodes(List<string> currencies)
        {
            await _cache.SetStringAsync("codes", string.Join(",", currencies));
        }
    }
}
