using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTrading.Repository
{
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
            var currentLot = await _ctx.Lots.Include(u => u.Owner).
                Include(u => u.Trade).FirstOrDefaultAsync(u => u.Id == lotId);
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
            currentLot = lot;
            await _ctx.SaveChangesAsync();
            return currentLot;
        }
    }
}
