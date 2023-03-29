using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTrading.Repository
{
    // TODO: Реализовать репозиторий для сущности Lot
    public class LotRepository : ILotRepository
    {
        private readonly DataContext _ctx;
        public LotRepository(DataContext context)
        {
            _ctx = context;
        }

        public async Task<Lot> CreateLotAsync(Lot lot)
        {
            var createdLot = await _ctx.AddAsync(lot);
            await _ctx.SaveChangesAsync();
            return createdLot.Entity;
        }

        public async Task<Lot> DeleteLotAsync(int lotId)
        {
            var deletedLot = await _ctx.Lots.FindAsync(lotId);
            _ctx.Lots.Remove(deletedLot);
            await _ctx.SaveChangesAsync();
            return deletedLot;        }

        public async Task<Lot> GetLotAsync(int lotId)
        {
            var currentLot = await _ctx.Lots.FindAsync(lotId);
            return currentLot;
        }

        public async Task<ICollection<Lot>> GetLotsAsync()
        {
            var lots = await _ctx.Lots.ToListAsync();
            return lots;
        }

        public async Task<Lot> UpdateLotAsync(int lotId, Lot lot)
        {
            var currentLot = await _ctx.Lots.FindAsync(lotId);
            currentLot.Currency = lot.Currency;
            currentLot.CurrencyAmount = lot.CurrencyAmount;
            currentLot.Price = lot.Price;
            await SaveAsync();
            return currentLot;
        }
        public async Task<bool> SaveAsync()
        {
            var saved = await _ctx.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
