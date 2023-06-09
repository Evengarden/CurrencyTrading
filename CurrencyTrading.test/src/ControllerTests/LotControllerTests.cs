﻿using System.Security.Claims;
using CurrencyTrading.Controllers;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CurrencyTrading.test.src.ControllerTests
{
    public class LotControllerTests
	{
        private readonly LotController _lotController;
        private readonly Mock<ILotService> _lotService;
        private readonly Mock<ICurrencyService> _currencyService;
        private readonly Mock<IAuthService> _authService;
        private readonly DataContext _ctx;
        private readonly User _user;
        public LotControllerTests()
		{
            PrepareTestsData.InitDbCtx(out _ctx);
            PrepareTestsData.InitUserInDb(_ctx, out _user);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                    new Claim("ID", _user.Id.ToString()),
                }, "mock"));
            _lotService = new Mock<ILotService>();
            _currencyService = new Mock<ICurrencyService>();
            _authService = new Mock<IAuthService>();
            _lotController = new LotController(_lotService.Object,_currencyService.Object, _authService.Object);
            _lotController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = claims }
            };
        }

        [Fact]
        public async Task GetLot_ShouldReturnIActionResultWithFindedLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Id=1,
                Status=Statuses.Created,
                Automatch=Automatch.Off,
                Currency="USD",
                CurrencyAmount=10,
                Owner=_user,
                Price=1000,
                Type=Types.Sold,
                OwnerId=_user.Id
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotController.GetLot(lot.Id);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetLots_ShouldReturnIActionResultWithFindedLots()
        {
            //arrange
            List<Lot> lots = new List<Lot>{ new Lot
                {
                    Id = 1,
                    Status = Statuses.Created,
                    Automatch = Automatch.Off,
                    Currency = "USD",
                    CurrencyAmount = 10,
                    Owner = _user,
                    Price = 1000,
                    Type = Types.Sold,
                    OwnerId = _user.Id
                }
            };
            _lotService.Setup(l => l.GetLots()).ReturnsAsync(lots);
            //act
            var result = await _lotController.GetLots();
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnIActionResultWithUpdatedLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch=Automatch.Off,
                Currency="KRW",
                CurrencyAmount=10000,
                Price=10,
                Type = Types.Buy
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l=>l.UpdateLot(lot.Id,lotDTO,_user.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotController.UpdateLot(lot.Id,lotDTO);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnCustomErrorNotEnoughBalanceForSold()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.UpdateLot(lot.Id, lotDTO, _user.Id)).ThrowsAsync(new NotEnoughBalanceForSold());

            //act
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async()=> await _lotController.UpdateLot(lot.Id, lotDTO));
        }
        [Fact]
        public async Task UpdateLot_ShouldReturnCustomErrorNotEnoughBalanceForBuy()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.UpdateLot(lot.Id, lotDTO, _user.Id)).ThrowsAsync(new NotEnoughBalanceForBuy());

            //act
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _lotController.UpdateLot(lot.Id, lotDTO));
        }

        [Fact]
        public async Task UpdateLot_ShouldReturnCustomErrorLotAlreadySolded()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.UpdateLot(lot.Id, lotDTO, _user.Id)).ThrowsAsync(new LotAlreadySolded());

            //act
            Assert.ThrowsAsync<LotAlreadySolded>(async () => await _lotController.UpdateLot(lot.Id, lotDTO));
        }

        [Fact]
        public async Task DeleteLot_ShouldReturnIActionResultWithDeletedLot()
        {
            //arrange
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _lotService.Setup(l => l.DeleteLot(lot.Id)).ReturnsAsync(lot);
            //act
            var result = await _lotController.DeleteLot(lot.Id);
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
                Status = Statuses.Created,
                Automatch = Automatch.Off,
                Currency = "USD",
                CurrencyAmount = 10,
                Owner = _user,
                Price = 1000,
                Type = Types.Sold,
                OwnerId = _user.Id
            };
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.DeleteLot(lot.Id)).ThrowsAsync(new LotAlreadySolded());
            //act
            Assert.ThrowsAsync<LotAlreadySolded>(async () => await _lotController.DeleteLot(lot.Id));
        }
        [Fact]
        public async Task CreateLot_ShouldReturnIActionResultWithCreatedLot()
        {
            //arrange
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = lotDTO.Automatch,
                Currency = lotDTO.Currency,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Owner = _user,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                OwnerId = _user.Id
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.CreateLot(_user.Id, lotDTO)).ReturnsAsync(lot);
            //act
            var result = await _lotController.CreateLot(lotDTO);
            //assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task CreateLot_ShouldReturnCustomErrorNotEnoughBalanceForSold()
        {
            //arrange
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = lotDTO.Automatch,
                Currency = lotDTO.Currency,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Owner = _user,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                OwnerId = _user.Id
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.CreateLot(_user.Id, lotDTO)).ThrowsAsync(new NotEnoughBalanceForSold());
            //act
            Assert.ThrowsAsync<NotEnoughBalanceForSold>(async () => await _lotController.CreateLot(lotDTO));
        }
        [Fact]
        public async Task CreateLot_ShouldReturnNotEnoughBalanceForBuy()
        {
            //arrange
            LotDTO lotDTO = new LotDTO
            {
                Automatch = Automatch.Off,
                Currency = "KRW",
                CurrencyAmount = 10000,
                Price = 10,
                Type = Types.Buy
            };
            Lot lot = new Lot
            {
                Id = 1,
                Status = Statuses.Created,
                Automatch = lotDTO.Automatch,
                Currency = lotDTO.Currency,
                CurrencyAmount = lotDTO.CurrencyAmount,
                Owner = _user,
                Price = lotDTO.Price,
                Type = lotDTO.Type,
                OwnerId = _user.Id
            };
            _lotService.Setup(l => l.GetLot(lot.Id)).ReturnsAsync(lot);
            _currencyService.Setup(i => i.CheckCurrencyExist(It.IsAny<string>())).ReturnsAsync("USD");
            _lotService.Setup(l => l.CreateLot(_user.Id, lotDTO)).ThrowsAsync(new NotEnoughBalanceForBuy());
            //act
            Assert.ThrowsAsync<NotEnoughBalanceForBuy>(async () => await _lotController.CreateLot(lotDTO));
        }
    }
}

