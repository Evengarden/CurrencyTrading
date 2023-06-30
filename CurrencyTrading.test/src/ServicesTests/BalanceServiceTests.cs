using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Services;
using Moq;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class BalanceServiceTests
    {
        private readonly BalanceService _balanceService;
        private readonly DataContext _ctx;
        private readonly User _user;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IBalanceRepository> _balanceRepository;

        public BalanceServiceTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx,out _user);
            _userRepository = new Mock<IUserRepository>();
            _balanceRepository = new Mock<IBalanceRepository>();
            _balanceService = new BalanceService(_balanceRepository.Object, _userRepository.Object);
        }

        [Fact]
        public async Task AddBalance_ShouldReturnCreatedBalance()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            BalanceDTO balanceDTO = new BalanceDTO
            {
                Amount = 1,
                Currency = "JPY"
            };
            Balance balance = new Balance
            {
                Amount = balanceDTO.Amount,
                Currency = balanceDTO.Currency,
                User = _user
            };
            _balanceRepository.Setup(b => b.CreateBalanceAsync(It.IsAny<Balance>())).ReturnsAsync(balance);
            //act
            var result = await _balanceService.AddBalance(_user.Id, balanceDTO);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddBalance_ShouldReturnCreatedBalance_AssertAmountIsEqual()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            BalanceDTO balanceDTO = new BalanceDTO
            {
                Amount = 1,
                Currency = "JPY"
            };
            Balance balance = new Balance
            {
                Amount = balanceDTO.Amount,
                Currency = balanceDTO.Currency,
                User = _user
            };
            _balanceRepository.Setup(b => b.CreateBalanceAsync(It.IsAny<Balance>())).ReturnsAsync(balance);
            //act
            var result = await _balanceService.AddBalance(_user.Id, balanceDTO);
            //assert
            Assert.Equal(result.Amount, balanceDTO.Amount);
        }
        [Fact]
        public async Task AddBalance_ShouldReturnCreatedBalance_AssertCurrencyIsEqual()
        {
            //arrange
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            BalanceDTO balanceDTO = new BalanceDTO
            {
                Amount = 1,
                Currency = "JPY"
            };
            Balance balance = new Balance
            {
                Amount = balanceDTO.Amount,
                Currency = balanceDTO.Currency,
                User = _user
            };
            _balanceRepository.Setup(b => b.CreateBalanceAsync(It.IsAny<Balance>())).ReturnsAsync(balance);
            //act
            var result = await _balanceService.AddBalance(_user.Id, balanceDTO);
            //assert
            Assert.Equal(result.Currency, balanceDTO.Currency);
        }

        [Fact]
        public async Task CheckBalance_ShouldReturnNotNullUserBalanceCollection()
        {
            //arrange
            _user.Balance = new List<Balance> {
                new Balance {
                Amount = 100,
                Currency ="USD",
                User = _user
                }
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            //act
            var balance = await _balanceService.CheckBalance(_user.Id);
            //assert
            Assert.NotNull(balance);
        }
        [Fact]
        public async Task CheckBalance_ShouldReturnNotEmptyUserBalanceCollection()
        {
            //arrange
            _user.Balance = new List<Balance> {
                new Balance {
                Amount = 100,
                Currency ="USD",
                User = _user
                }
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            //act
            var balance = await _balanceService.CheckBalance(_user.Id);
            //assert
            Assert.NotEmpty(balance);
        }
    }
}
