using AutoMapper;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.DAL.Mapping;
using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
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
        private readonly Mock<IBalanceCalculationService> _balanceCalculationService;
        private readonly IMapper _mapper;
        public LotServiceTests()
        {
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx, out _user);
            PrepareTestsData.InitUserBalance(_user);
            _userRepository = new Mock<IUserRepository>();
            _lotRepository = new Mock<ILotRepository>();
            _balanceCalculationService = new Mock<IBalanceCalculationService>();
            var myProfile = new AppMappingProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
            _lotService = new LotService(_userRepository.Object, _lotRepository.Object, _balanceCalculationService.Object, _mapper);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedSoldLot()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateLot_ShouldReturnCreatedSoldLot_AssertPriceEquals()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.Equal(result.Price, lotDTO.Price);
        }

        [Fact]
        public async Task CreateLot_ShouldReturnCreatedSoldLot_AssertCurrencyEquals()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.Equal(result.Currency, lotDTO.Currency);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedSoldLot_AssertCurrencyAmountEquals()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedBuyLot()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedBuyLot_AssertPriceEquals()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.Equal(result.Price, lotDTO.Price);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedBuyLot_AssertCurrencyEquals()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.Equal(result.Currency, lotDTO.Currency);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCreatedBuyLot_AssertCurrencyAmountEquals()
        {
            //arrange
            CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act
            var result = await _lotService.CreateLot(_user.Id, lotDTO);
            //assert
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }

        [Fact]
        public async Task CreateLot_ShouldReturnCustomErrorNotEnoughBalanceForSoldLot()
        {
            //arrange
            CreateLot_PrepareDataNotEnoughBalance(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCustomErrorNotEnoughBalanceForBuyLot()
        {
            //arrange
            CreateLot_PrepareDataNotEnoughBalance(out Lot lot, out LotDTO lotDTO);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.CreateLotAsync(It.IsAny<Lot>())).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        [Fact]
        public async Task DeleteLot_ShouldReturnDeletedLot()
        {
            //arrange
            DeleteLot_PrepareData(out Lot lot, false);
            _lotRepository.Setup(l => l.GetLotAsync(lot.Id)).ReturnsAsync(lot);
            _lotRepository.Setup(l => l.DeleteLotAsync(lot.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotService.DeleteLot(lot.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeleteLot_ShouldReturnCustomErrorLotAlreadySolded()
        {
            //arrange
            DeleteLot_PrepareData(out Lot lot, true);
            _lotRepository.Setup(l => l.GetLotAsync(lot.Id)).ReturnsAsync(lot);
            _lotRepository.Setup(l => l.DeleteLotAsync(lot.Id)).ReturnsAsync(lot);
            //act + assert
            Assert.ThrowsAsync<LotAlreadySolded>(async () => await _lotService.DeleteLot(lot.Id));
        }
        [Fact]
        public async Task GetLot_ShouldReturnFindedLot()
        {
            //arrange
            GetLot_PrepareData(out Lot lot);
            _lotRepository.Setup(l => l.GetLotAsync(lot.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotService.GetLot(lot.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetLots_ShouldReturnNotNullCollectionOfAllLots()
        {
            //arrange
            GetLot_PrepareData(out Lot lot);
            List<Lot> lots = new List<Lot> { lot };
            _lotRepository.Setup(l => l.GetLotsAsync()).ReturnsAsync(lots);
            //act
            var result = await _lotService.GetLots();
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetLots_ShouldReturnNotEmptyCollectionOfAllLots()
        {
            //arrange
            GetLot_PrepareData(out Lot lot);
            List<Lot> lots = new List<Lot> { lot };
            _lotRepository.Setup(l => l.GetLotsAsync()).ReturnsAsync(lots);
            //act
            var result = await _lotService.GetLots();
            //assert
            Assert.NotEmpty(result);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedSoldLot()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, true, false);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedSoldLot_AssertPriceEquals()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, true, false);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.Equal(result.Price, lotDTO.Price);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedSoldLot_AssertCurrencyEquals()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, true, false);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.Equal(result.Currency, lotDTO.Currency);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedSoldLot_AssertCurrencyAmountEquals()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, true, false);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedBuyLot()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, true, false);
            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedBuyLot_AssertPriceEquals()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, false, false);

            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.Equal(result.Price, lotDTO.Price);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedBuyLot_AssertCurrencyEquals()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, false, false);

            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.Equal(result.Currency, lotDTO.Currency);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnUpdatedBuyLot_AssertCurrencyAmountEquals()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, false, false);

            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act
            var result = await _lotService.UpdateLot(oldLot.Id, lotDTO, _user.Id);
            //assert
            Assert.Equal(result.CurrencyAmount, lotDTO.CurrencyAmount);
        }
        [Fact]
        public async Task UpdatedLot_ShouldReturnCustomErrorNotEnoughBalanceForSoldLot()
        {
            //arrange
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, true, true);

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
            UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, false, true);

            _userRepository.Setup(u => u.GetUserAsync(_user.Id)).ReturnsAsync(_user);
            _lotRepository.Setup(l => l.UpdateLotAsync(oldLot.Id, oldLot)).ReturnsAsync(newLot);
            _lotRepository.Setup(l => l.GetLotAsync(oldLot.Id)).ReturnsAsync(oldLot);
            //act + assert
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _lotService.CreateLot(_user.Id, lotDTO));
        }
        private void CreateLot_PrepareData(out Lot lot, out LotDTO lotDTO)
        {
            lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 100,
                Currency = "USD",
                Price = 10,
                Owner = _user,
            };
            lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Sold
            };
        }

        private void CreateLot_PrepareDataNotEnoughBalance(out Lot lot, out LotDTO lotDTO)
        {
            lot = new Lot
            {
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "USD",
                Price = 10,
                Owner = _user,
            };
            lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 100,
                Price = 10,
                Type = Types.Sold
            };
        }

        private void DeleteLot_PrepareData(out Lot lot, bool solded)
        {
            lot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user,
                Status = solded ? Statuses.Solded : Statuses.Created
            };
        }

        private void GetLot_PrepareData(out Lot lot)
        {
            lot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = 1000000,
                Currency = "KRW",
                Price = 10,
                Owner = _user
            };
        }

        private void UpdateLot_PrepareData(out Lot oldLot, out LotDTO lotDTO, out Lot newLot, bool isSold, bool isError)
        {
            oldLot = new Lot
            {
                Id = 1,
                Automatch = Automatch.Off,
                CurrencyAmount = isError ? 5000000 : 10000,
                Currency = "USD",
                Price = 10,
                Type = Types.Sold,
                Status = Statuses.Created,
                Owner = _user,
            };
            lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 50,
                Price = 1000,
                Type = isSold ? Types.Sold : Types.Buy
            };
            newLot = new Lot
            {
                Id = 1,
                Automatch = lotDTO.Automatch,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Currency = lotDTO.Currency,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                Owner = _user,
            };
        }
    }
}
