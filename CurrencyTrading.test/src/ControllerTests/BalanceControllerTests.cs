using System;
using System.Security.Claims;
using CurrencyTrading.Controllers;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyTrading.test.src.ControllerTests
{
	public class BalanceControllerTests
	{
		private readonly BalanceController _balanceController;
		private readonly Mock<IBalanceService> _balanceService;
        private readonly DataContext _ctx;
        private readonly User _user;
        public BalanceControllerTests()
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
            _balanceService = new Mock<IBalanceService>();
            _balanceController = new BalanceController(_balanceService.Object);
            _balanceController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext= new DefaultHttpContext() { User = claims }
            };
		}

		[Fact]
		public async Task GetUserBalance_ShouldReturnIActionResultWithUserBalance()
		{
            //arrange
            _user.Balance = new List<Balance> {
                new Balance {
                Amount = 100,
                Currency ="USD",
                User = _user
                }
            };
            _balanceService.Setup(b=>b.CheckBalance(_user.Id)).ReturnsAsync(_user.Balance);
            //act
            var result = await _balanceController.GetUserBalance();
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateBalance_ShouldReturnIActionResultWithCreatedBalance()
        {
            //arrange
            BalanceDTO balanceDTO = new BalanceDTO
            {
                Amount = 100,
                Currency = "USD"
            };
            Balance balance = new Balance
            {
                Amount = balanceDTO.Amount,
                Currency = balanceDTO.Currency,
                User = _user
            };
            _balanceService.Setup(b => b.AddBalance(_user.Id,balanceDTO)).ReturnsAsync(balance);
            //act
            var result = await _balanceController.CreateUserBalance(balanceDTO);
            //assert
            Assert.NotNull(result);
        }
    }
}

