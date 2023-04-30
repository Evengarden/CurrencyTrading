using Moq;
using CurrencyTrading.Controllers;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.Interfaces;

namespace CurrencyTrading.test.src.ControllerTests
{
	public class IntegrationControllerTests
	{
		private readonly IntegrationController _integrationController;
		private readonly Mock<IIntegrationService> _integrationService;
        private readonly DataContext _ctx;
        private readonly User _user;
        public IntegrationControllerTests() 
		{
            var dbOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _ctx = new DataContext(dbOptions);
            _ctx.Database.EnsureCreated();
            _user = _ctx.Users.Add(new User
            {
                Login = "test",
                Password = HashPassword.HashPass("test")
            }).Entity;
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("ID", _user.Id.ToString()),
                }, "mock"));
            _integrationService = new Mock<IIntegrationService>();
			_integrationController = new IntegrationController(_integrationService.Object);
            _integrationController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
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
            _integrationService.Setup(i => i.GetCurrencyFromRedis()).ReturnsAsync(currencyDTOs);
            //act
            var result = await _integrationController.GetCurrency();
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
            var result = await _integrationController.CalculateCurrencyPrice(balanceDTO);
            Assert.NotNull(result);
        }
    }
}

