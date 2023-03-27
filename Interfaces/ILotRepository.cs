using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface ILotRepository
    {
        Task<Lot> GetLotAsync(int lotId);
        Task<ICollection<Lot>> GetLotsAsync();
        Task<Lot> CreateLotAsync(Lot lot);
        Task<Lot> UpdateLotAsync(Lot lot);
        Task<Lot> DeleteLotAsync(Lot lot);
        Task<bool> SaveAsync();
    }
}
