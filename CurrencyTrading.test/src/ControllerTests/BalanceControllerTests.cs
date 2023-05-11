using System;
using System.Security.Claims;
using CurrencyTrading.Controllers;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
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
        private readonly Mock<IAuthService> _authService;
        private readonly DataContext _ctx;
        private readonly User _user;
        public BalanceControllerTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx,out _user);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("ID", _user.Id.ToString()),
                }, "mock"));
            _balanceService = new Mock<IBalanceService>();
            _authService = new Mock<IAuthService>();
            _balanceController = new BalanceController(_balanceService.Object,_authService.Object);
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

