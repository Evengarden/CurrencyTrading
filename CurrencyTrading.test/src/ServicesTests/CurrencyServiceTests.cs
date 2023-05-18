using CurrencyTrading.Client.Interfaces;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.DAL.Interfaces;
using CurrencyTrading.Data;
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
    public class CurrencyServiceTests
    {
        private readonly CurrencyService _currencyService;
        private readonly Mock<ICurrencyRepository> _currencyRepository;
        private readonly Mock<ICbApiClient> _client;
        private readonly string codes = "AUD,AZN,GBP,AMD,BYN,BGN,BRL,HUF,VND,HKD,GEL,DKK,AED,USD,EUR," +
            "EGP,INR,IDR,KZT,CAD,QAR,KGS,CNY,MDL,NZD,NOK,PLN,RON,XDR,SGD,TJS,THB,TRY,TMT,UZS,UAH,CZK," +
            "SEK,CHF,RSD,ZAR,KRW,JPY";
        private readonly CurrencyDTO _currencyDTO;
        public CurrencyServiceTests()
        {
            _currencyRepository = new Mock<ICurrencyRepository>();
            _client = new Mock<ICbApiClient>();
            _currencyRepository.Setup(c => c.GetCurrencyCodes()).ReturnsAsync(codes);
            _currencyDTO = new CurrencyDTO
            {
                CurrencyCode = "AUD",
                CurrencyNominal = 1,
                CurrencyPrice = 1
            };
            _currencyRepository.Setup(c => c.GetCurrency(It.Is<string>(s => codes.Split(",", StringSplitOptions.None)
            .Any(s.Contains))))
                .ReturnsAsync(JsonConvert.SerializeObject(_currencyDTO));
            _currencyService = new CurrencyService(_currencyRepository.Object,_client.Object);
        }

        [Fact]
        public async Task GetCurrencyFromRedis_ShouldReturnNotEmptyTestCollection()
        {
            //act
            var result = await _currencyService.GetCurrency();
            //assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CheckCurrencyExist_ShouldReturnNotNullTestCurrency()
        {
            //act
            var result = await _currencyService.CheckCurrencyExist("AUD");
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CheckCurrencyExist_ShouldReturnNotEmptyTestCurrency()
        {
            //act
            var result = await _currencyService.CheckCurrencyExist("AUD");
            //assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CheckCurrencyExist_ShouldReturnCurrencyNotFoundException()
        {
            //act + assert
            Assert.ThrowsAsync<CurrencyNotFound>(async () => await _currencyService.CheckCurrencyExist("test"));
        }

        [Fact]
        public async Task CalculateLotPrice_ShouldReturnCalculatedCurrencyPrice()
        {
            //arrange
            int currencyAmount = 10;
            //act
            var result = await _currencyService.CalculateLotPrice("AUD",currencyAmount);
            //assert
            Assert.Equal(result, (_currencyDTO.CurrencyPrice / _currencyDTO.CurrencyNominal) * currencyAmount);
        }

        [Fact]
        public async Task CalculateLotPrice_ShouldReturnCurrencyNotFoundException()
        {
            //arrange
            int currencyAmount = 10;
            //act + assert
            Assert.ThrowsAsync<CurrencyNotFound>(async () => await _currencyService.CalculateLotPrice("test", currencyAmount));
        }
    }
}
