using Moq;
using CurrencyTrading.Controllers;
using CurrencyTrading.Data;
using CurrencyTrading.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.Interfaces;

namespace CurrencyTrading.test.src.ControllerTests
{
    public class CurrencyControllerTests
	{
		private readonly CurrencyController _currencyController;
		private readonly Mock<ICurrencyService> _integrationService;
        private readonly DataContext _ctx;
        private readonly User _user;
        public CurrencyControllerTests() 
		{
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx, out _user);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("ID", _user.Id.ToString()),
                }, "mock"));
            _integrationService = new Mock<ICurrencyService>();
			_currencyController = new CurrencyController(_integrationService.Object);
            _currencyController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claims }
            };
        }

        [Fact]
        public async Task GetCurrency_ShouldReturnIActionResultWithCurrenies()
        {
            //arrange
            List<CurrencyDTO> currencyDTOs = new List<CurrencyDTO>
            {
                new CurrencyDTO
                {
                    CurrencyCode = "USD",
                    CurrencyNominal=1,
                    CurrencyPrice=70
                }
            };
            _integrationService.Setup(i => i.GetCurrency()).ReturnsAsync(currencyDTOs);
            //act
            var result = await _currencyController.GetCurrency();
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CalculateLotPrice_ShouldReturnIActionResultWithCalculatedPrice()
        {
            //arrange
            BalanceDTO balanceDTO = new BalanceDTO
            {
                Amount=1,
                Currency="USD"
            };

            _integrationService.Setup(i=>i.CalculateLotPrice(balanceDTO.Currency,balanceDTO.Amount)).ReturnsAsync(100);
            //act
            var result = await _currencyController.CalculateCurrencyPrice(balanceDTO);
            Assert.NotNull(result);
        }
    }
}

