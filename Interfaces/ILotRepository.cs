using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface ILotRepository
    {
        Lot GetLot(int lotId);
        ICollection<Lot> GetLots();
        bool CreateLot(Lot lot);
        bool UpdateLot(Lot lot);
        bool DeleteLot(Lot lot);
        bool Save();
    }
}
