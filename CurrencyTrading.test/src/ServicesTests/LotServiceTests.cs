using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CurrencyTrading.test.src.ServicesTests
{
    public class LotServiceTests
    {
        private readonly LotService _lotService;
        private readonly DataContext _ctx;
        private readonly User _user;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ILotRepository> _lotRepository;
        public LotServiceTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx, out _user);
            PrepareTestsData.InitUserBalance(_user);
            _userRepository = new Mock<IUserRepository>();
            _lotRepository = new Mock<ILotRepository>();
            _lotService = new LotService(_userRepository.Object,_lotRepository.Object);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedSoldLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Sold
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id,lotDTO);
            //assert
            Assert.NotNull(result);
            Assert.Equal(result.Price,lotDTO.Price);
            Assert.Equal(result.Currency, lotDTO.Currency);
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedBuyLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Buy
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.NotNull(result);
            Assert.Equal(result.Price, lotDTO.Price);
            Assert.Equal(result.Currency, lotDTO.Currency);
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCustomErrorNotEnoughBalanceForSoldLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "USD",
                Price = 10,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Sold
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async () => await _lotService.CreateLot(_user.Id,lotDTO));
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCustomErrorNotEnoughBalanceForBuyLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "USD",
                Price = 10,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Buy
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCustomErrorBalanceNotFound()
        {
            //arrange
            Lot lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Buy
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<BalanceDoesNotExist>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        [Fact]
        public async Task DeleteLot_ShouldReturnDeletedLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user,
                Status = Statuses.Created
            };
            _lotRepository.Setup(l => l.GetLotAsync(lot.Id)).ReturnsAsync(lot);
            _lotRepository.Setup(l=> l.DeleteLotAsync(lot.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotService.DeleteLot(lot.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeleteLot_ShouldReturnCustomErrorLotAlreadySolded()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user,
                Status = Statuses.Solded
            };
            _lotRepository.Setup(l => l.GetLotAsync(lot.Id)).ReturnsAsync(lot);
            _lotRepository.Setup(l => l.DeleteLotAsync(lot.Id)).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<LotAlreadySolded>(async () => await _lotService.DeleteLot(lot.Id));
        }
        [Fact]
        public async Task GetLot_ShouldReturnFindedLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user
            };
            _lotRepository.Setup(l => l.GetLotAsync(lot.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotService.GetLot(lot.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetLots_ShouldReturnAllLots()
        {
            //arrange
            List<Lot> lot = new List<Lot> { new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user
            }};
            _lotRepository.Setup(l => l.GetLotsAsync()).ReturnsAsync(lot);
            //act
            var result = await _lotService.GetLots();
            //assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedSoldLot()
        {
            //arrange
            Lot oldLot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Type = Types.Sold,
                Status = Statuses.Created,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 50,
                Price = 1000,
                Type = Types.Sold
            };
            Lot newLot = new Lot
            {
                Id = 1,
                Automatch = lotDTO.Automatch,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Currency = lotDTO.Currency,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                Owner = _user,
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO,_user.Id);
            //assert
            Assert.NotNull(result);
            Assert.Equal(result.Price, lotDTO.Price);
            Assert.Equal(result.Currency, lotDTO.Currency);
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedBuyLot()
        {
            //arrange
            Lot oldLot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Type = Types.Sold,
                Status = Statuses.Created,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 50,
                Price = 1000,
                Type = Types.Buy
            };
            Lot newLot = new Lot
            {
                Id = 1,
                Automatch = lotDTO.Automatch,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Currency = lotDTO.Currency,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                Owner = _user,
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.NotNull(result);
            Assert.Equal(result.Price, lotDTO.Price);
            Assert.Equal(result.Currency, lotDTO.Currency);
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task UpdatedLot_ShouldReturnCustomErrorNotEnoughBalanceForSoldLot()
        {
            //arrange
            Lot oldLot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Type = Types.Sold,
                Status = Statuses.Created,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 5000000,
                Price = 1000,
                Type = Types.Sold
            };
            Lot newLot = new Lot
            {
                Id = 1,
                Automatch = lotDTO.Automatch,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Currency = lotDTO.Currency,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                Owner = _user,
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        [Fact]
        public async Task UpdatedLot_ShouldReturnCustomErrorNotEnoughBalanceForBuyLot()
        {
            //arrange
            Lot oldLot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Type = Types.Sold,
                Status = Statuses.Created,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 50,
                Price = 1000,
                Type = Types.Buy
            };
            Lot newLot = new Lot
            {
                Id = 1,
                Automatch = lotDTO.Automatch,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Currency = lotDTO.Currency,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                Owner = _user,
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        [Fact]
        public async Task UpdatedLot_ShouldReturnCustomErrorBalanceNotFound()
        {
            //arrange
            Lot oldLot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Type = Types.Sold,
                Status = Statuses.Created,
                Owner = _user,
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 50,
                Price = 1000,
                Type = Types.Buy
            };
            Lot newLot = new Lot
            {
                Id = 1,
                Automatch = lotDTO.Automatch,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Currency = lotDTO.Currency,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                Owner = _user,
            };
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act + assert
            Assert.ThrowsAsync<BalanceDoesNotExist>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
    }
}
