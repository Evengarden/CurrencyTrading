using CurrencyTrading.Data;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;

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

        public Task<Lot> CreateLotAsync(Lot lot)
        {
            throw new NotImplementedException();
        }

        public Task<Lot> DeleteLotAsync(Lot lot)
        {
            throw new NotImplementedException();
        }

        public Task<Lot> GetLotAsync(int lotId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Lot>> GetLotsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Lot> UpdateLotAsync(Lot lot)
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
