using AutoMapper;
using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;

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
