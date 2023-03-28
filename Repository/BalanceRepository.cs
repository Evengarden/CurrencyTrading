using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using System.Runtime.InteropServices;

namespace CurrencyTrading.Repository
{
    // TODO: Реализовать репозиторий для сущности Balance
    public class BalanceRepository : IBalanceRepository
    {
        private readonly DataContext _ctx;
        public BalanceRepository(DataContext context) 
        {
            _ctx = context;
        }

        public async Task<Balance> CreateBalanceAsync(Balance balance)
        {
            var createdBalance = await _ctx.AddAsync(balance);
            await _ctx.SaveChangesAsync();
            return createdBalance.Entity;
        }

        public async Task<Balance> DeleteBalanceAsync(int balanceId)
        {
            var deletedBalance = await _ctx.Balances.FindAsync(balanceId);
            _ctx.Balances.Remove(deletedBalance);
            await _ctx.SaveChangesAsync();
            return deletedBalance;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<Balance> UpdateBalanceAsync(int balanceId, Balance balance)
        {
            var currentBalance = await _ctx.Balances.FindAsync(balanceId);
            currentBalance.Currency = balance.Currency;
            currentBalance.Amount = balance.Amount;
            await SaveAsync();
            return currentBalance;
        }
    }
}
