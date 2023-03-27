using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;

namespace CurrencyTrading.Repository
{
    // TOOD: Реализовать репозиторий для сущности Trade
    public class TradeRepository : ITradeRepository
    {
        private readonly DataContext _ctx;
        public TradeRepository(DataContext context) 
        {
            _ctx = context;
        }

        public async Task<Trade> CreateTradeAsync(Trade trade)
        {
            throw new NotImplementedException();
        }

        public async Task<Trade> GetTradeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async  Task<ICollection<Trade>> GetTradesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
