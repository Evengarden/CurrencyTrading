using CurrencyTrading.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTradig.CbClient.Interfaces
{
    public interface ICbApiClient
    {
        Task<ICollection<CurrencyDTO>> sendRequestToCb();
    }
}
