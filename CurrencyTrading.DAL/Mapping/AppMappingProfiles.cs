using AutoMapper;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyTrading.DAL.Mapping
{
    public class AppMappingProfiles : Profile
    {
        public AppMappingProfiles()
        {
            CreateMap<Balance,BalanceDTO>().ReverseMap();
            CreateMap<Lot,LotDTO>().ReverseMap();
            CreateMap<Trade,TradeDTO>().ReverseMap();
            CreateMap<User,UserDTO>().ReverseMap();
        }
    }
}
