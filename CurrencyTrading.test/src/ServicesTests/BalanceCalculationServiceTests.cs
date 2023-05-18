using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class BalanceCalculationServiceTests
    {
        private readonly BalanceCalculationService _balanceCalculationService;
        private readonly DataContext _ctx;
        private readonly User _buyer;
        private readonly User _owner;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IBalanceRepository> _balanceRepository;
        public BalanceCalculationServiceTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx, out _buyer);
            PrepareTestsData.InitUserInDb(_ctx, out _owner);
            PrepareTestsData.InitUserBalance(_buyer);
            PrepareTestsData.InitUserBalance(_owner);
            _userRepository = new Mock<IUserRepository>();
            _balanceRepository = new Mock<IBalanceRepository>();
            _balanceCalculationService = new BalanceCalculationService(_balanceRepository.Object);
        }

        [Fact]
        public async Task CalculateBalance_ShouldReturnCalculatedUserBalanceIfLotTypeSold()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _owner,
            };
            Balance newBalance = new Balance
            {
                Currency = "USD",
                Amount = 0,
                User = _buyer
            };
            _balanceRepository.Setup(b => b.CreateBalanceAsync(newBalance)).ReturnsAsync(newBalance);
            //act
            var balance = await _balanceCalculationService.CalculateBalance(_buyer,_owner,lot);
            //assert
            Assert.NotNull(balance);
        }
        [Fact]
        public async Task CalculateBalance_ShouldReturnCalculatedUserBalanceIfLotTypeBuy()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _owner,
                Type = Types.Buy
            };
            Balance newBalance = new Balance
            {
                Currency = "USD",
                Amount = 0,
                User = _buyer
            };
            _balanceRepository.Setup(b => b.CreateBalanceAsync(newBalance)).ReturnsAsync(newBalance);
            //act
            var balance = await _balanceCalculationService.CalculateBalance(_buyer, _owner, lot);
            //assert
            Assert.NotNull(balance);
        }
        [Fact]
        public async Task CalculateBalance_ShouldReturnCustomErrorOwnerIsBuyer()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _owner,
            };
            Balance newBalance = new Balance
            {
                Currency = "USD",
                Amount = 0,
                User = _buyer
            };
            User user = _owner;
            _balanceRepository.Setup(b => b.CreateBalanceAsync(newBalance)).ReturnsAsync(newBalance);
            //act + assert
            Assert.ThrowsAsync<OwnerIsBuyer>(async () => await _balanceCalculationService.CalculateBalance(user, _owner, lot));
        }
        [Fact]
        public async Task CheckEnoughBalanceForSold_ShouldNotReturnError()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _owner,
            };
            //act
            var exception = Record.Exception(() => _balanceCalculationService.CheckEnoughBalanceForSold(_buyer,lot));
            //assert
            Assert.Null(exception);
            
        }
        [Fact]
        public async Task CheckEnoughBalanceForSold_ShouldReturnCustomErrorNotEnoughBalanceForSold()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "USD",
                Price = 1,
                Owner = _owner,
            };
            //act
            Assert.Throws<NotEnoughBalanceForSold>(() =>_balanceCalculationService.CheckEnoughBalanceForSold(_buyer, lot));
        }
        [Fact]
        public async Task CheckEnoughBalanceForBuy_ShouldNotReturnError()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _owner,
            };
            //act
            var exception = Record.Exception(() => _balanceCalculationService.CheckEnoughBalanceForBuy(_buyer, lot));
            //assert
            Assert.Null(exception);
        }
        [Fact]
        public async Task CheckEnoughBalanceForBuy_ShouldReturnCustomErrorNotEnoughBalanceForBuy()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 1000000000,
                Owner = _owner,
            };
            //act
            Assert.Throws<NotEnoughBalanceForBuy>(() => _balanceCalculationService.CheckEnoughBalanceForBuy(_buyer, lot));
        }
    }
}
