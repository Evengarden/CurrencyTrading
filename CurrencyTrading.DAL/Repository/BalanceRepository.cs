using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using System.Runtime.InteropServices;

namespace CurrencyTrading.Repository
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly DataContext _ctx;
        public BalanceRepository(DataContext context) 
        {
            _ctx = context;
        }

        public async Task<Balance> CreateBalanceAsync(Balance balance)
        {

            var createdBalance = await _ctx.Balances.AddAsync(balance);
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

        public async Task<Balance> UpdateBalanceAsync(int balanceId, Balance balance)
        {
            var currentBalance = await _ctx.Balances.FindAsync(balanceId);
            currentBalance = balance;
            await _ctx.SaveChangesAsync();
            return currentBalance;
        }
    }
}
