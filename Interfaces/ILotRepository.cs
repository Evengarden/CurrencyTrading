using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface ILotRepository
    {
        Task<Lot> GetLot(int lotId);
        Task<ICollection<Lot>> GetLots();
        Task<Lot> CreateLot(Lot lot);
        Task<Lot> UpdateLot(Lot lot);
        Task<Lot> DeleteLot(Lot lot);
        Task<bool> Save();
    }
}
