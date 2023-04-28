﻿using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Assert.Equal(result.Amount, balanceDTO.Amount);
            Assert.Equal(result.Currency, balanceDTO.Currency);
        }

        [Fact]
        public async Task CheckBalance_ShouldReturnUserBalance()
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
            Assert.NotEmpty(balance);
        }
    }
}
