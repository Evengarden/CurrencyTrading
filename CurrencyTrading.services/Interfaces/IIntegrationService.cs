using CurrencyTrading.DAL.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Interfaces
{
    public interface IIntegrationService
    {
        Task<ICollection<CurrencyDTO>> GetCurrencyFromRedis();

    }
}
