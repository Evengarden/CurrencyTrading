using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;

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
            throw new NotImplementedException();
        }

        public async Task<Balance> DeleteBalanceAsync(Balance balance)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<Balance> UpdateBalanceAsync(Balance balance)
        {
            throw new NotImplementedException();
        }
    }
}
