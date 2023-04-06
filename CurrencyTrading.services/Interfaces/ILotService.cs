using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Interfaces
{
    public interface ILotService
    {
        Task<Lot> CreateLot(int userId,LotDTO lot);
        Task<Lot> UpdateLot(int lotId,LotDTO lot,int userId);
        Task<Lot> GetLot(int lotId);
        Task<ICollection<Lot>> GetLots(int userId);
        Task<Lot> DeleteLot(int lotId);
    }
}
