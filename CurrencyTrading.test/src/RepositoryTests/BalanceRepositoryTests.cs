using CurrencyTrading.Data;
using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.test.src.RepositoryTests
{
    public class BalanceRepositoryTests : IDisposable
    {
        private readonly BalanceRepository _balanceRepository;
        private readonly DataContext _ctx;
        private readonly User _user;

        public BalanceRepositoryTests()
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
            _balanceRepository = new BalanceRepository(_ctx);
        }

        [Fact]
        public async Task BalanceRepository_ShouldReturnCreatedBalanceFromDb()
        {
            //arrange
            var balance = prepareBalanceData();
            //act
            var createdBalance = await _balanceRepository.CreateBalanceAsync(balance);
            //assert
            Assert.NotNull(createdBalance);
            Assert.NotNull(createdBalance.User);

            Dispose();
        }

        [Fact]
        public async Task BalanceRepository_ShouldNotCreateBalanceWithRequiredNullFieldFromDb()
        {
            //arrange
            var balance = prepareBalanceData();
            balance.Currency = null;
            //assert
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _balanceRepository.CreateBalanceAsync(balance));


            Dispose();
        }

        [Fact]
        public async Task BalanceRepository_ShouldReturnUpdatedBalanceFromDb()
        {
            //arrange
            var balance = prepareBalanceData();
            decimal previousAmount = balance.Amount;
            var createdBalance = await _balanceRepository.CreateBalanceAsync(balance);
            createdBalance.Amount = 200;
            //act
            var updatedBalance = await _balanceRepository.UpdateBalanceAsync(balance.Id, balance);
            //assert
            Assert.NotNull(updatedBalance);
            Assert.NotEqual(previousAmount, updatedBalance.Amount);

            Dispose();
        }

        [Fact]
        public async Task BalanceRepository_ShouldNotReturnDeletedBalanceFromDb()
        {
            //arrange
            var balance = prepareBalanceData();
            var createdBalance = await _balanceRepository.CreateBalanceAsync(balance);
            //act
            var deletedBalance = await _balanceRepository.DeleteBalanceAsync(createdBalance.Id);
            var foundedDeletedBalance = await _ctx.FindAsync<Balance>(deletedBalance.Id);
            //assert
            Assert.NotNull(deletedBalance);
            Assert.Null(foundedDeletedBalance);

            Dispose();
        }

        [Fact]
        public async Task BalanceRepository_ShouldNotDeleteNonExistedBalanceFromDb()
        {
            // arrange
            var nonExistingBalanceId = -1;
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>
                (async () => await _balanceRepository.DeleteBalanceAsync(nonExistingBalanceId));

            Dispose();
        }

        public Balance prepareBalanceData()
        {
            Balance balance = new Balance
            {
                Currency = "USD",
                Amount = 100,
                UserId = _user.Id
            };
            return balance;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
