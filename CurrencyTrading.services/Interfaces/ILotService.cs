using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;

namespace CurrencyTrading.services.Interfaces
{
    public interface ILotService
    {
        Task<Lot> CreateLot(int userId,LotDTO lot);
        Task<Lot> UpdateLot(int lotId,LotDTO lot,int userId);
        Task<Lot> GetLot(int lotId);
        Task<ICollection<Lot>> GetLots();
        Task<Lot> DeleteLot(int lotId);
    }
}
