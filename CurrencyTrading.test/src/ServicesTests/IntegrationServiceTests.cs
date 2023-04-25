using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class IntegrationServiceTests
    {
        private readonly IntegrationService _integrationService;
        private readonly Mock<IDistributedCache> _cache;
        private readonly string codes = "AUD,AZN,GBP,AMD,BYN,BGN,BRL,HUF,VND,HKD,GEL,DKK,AED,USD,EUR," +
            "EGP,INR,IDR,KZT,CAD,QAR,KGS,CNY,MDL,NZD,NOK,PLN,RON,XDR,SGD,TJS,THB,TRY,TMT,UZS,UAH,CZK," +
            "SEK,CHF,RSD,ZAR,KRW,JPY";
        private readonly CurrencyDTO _currencyDTO;
        public IntegrationServiceTests()
        {
            _cache = new Mock<IDistributedCache> { CallBase = true};
            _cache.Setup(c => c.GetAsync("codes",default)).ReturnsAsync(Encoding.ASCII.GetBytes(codes));
            _currencyDTO = new CurrencyDTO
            {
                CurrencyCode = "AUD",
                CurrencyNominal = 1,
                CurrencyPrice = 1
            };
            _cache.Setup(c => c.GetAsync(It.Is<string>(s => codes.Split(",",StringSplitOptions.None)
            .Any(s.Contains)), default))
                .ReturnsAsync(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(_currencyDTO)));
           
            _integrationService = new IntegrationService(_cache.Object);
        }

        [Fact]
        public async Task GetCurrencyFromRedis_ShouldReturnTestCollection()
        {
            //act
            var result = await _integrationService.GetCurrencyFromRedis();
            //assert
            Assert.NotEmpty(result);
            Assert.False(result.IsNullOrEmpty());
        }

        [Fact]
        public async Task CheckCurrencyExist_ShouldReturnTestCurrency()
        {
            //act
            var result = await _integrationService.CheckCurrencyExist("AUD");
            //assert
            Assert.NotEmpty(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CheckCurrencyExist_ShouldReturnCurrencyNotFoundException()
        {
            //act + assert
            Assert.ThrowsAsync<CurrencyNotFound>(async () => await _integrationService.CheckCurrencyExist("test"));
        }

        [Fact]
        public async Task CalculateLotPrice_ShouldReturnCalculatedCurrencyPrice()
        {
            //arrange
            int currencyAmount = 10;
            //act
            var result = await _integrationService.CalculateLotPrice("AUD",currencyAmount);
            //assert
            Assert.Equal(result, (_currencyDTO.CurrencyPrice / _currencyDTO.CurrencyNominal) * currencyAmount);
        }

        [Fact]
        public async Task CalculateLotPrice_ShouldReturnCurrencyNotFoundException()
        {
            //arrange
            int currencyAmount = 10;
            //act + assert
            Assert.ThrowsAsync<CurrencyNotFound>(async () => await _integrationService.CalculateLotPrice("test", currencyAmount));
        }
    }
}
