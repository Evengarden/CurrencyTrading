using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;

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
            var createdTrade = await _ctx.AddAsync(trade);
            await _ctx.SaveChangesAsync();
            return createdTrade.Entity;

        }
        public async Task<Trade> GetTradeAsync(int id)
        {
            var currentTrade = await _ctx.FindAsync<Trade>(id);
            return currentTrade;
        }

        public async  Task<ICollection<Trade>> GetTradesAsync()
        {
            var trades = await _ctx.Trades.ToListAsync();
            return trades;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
