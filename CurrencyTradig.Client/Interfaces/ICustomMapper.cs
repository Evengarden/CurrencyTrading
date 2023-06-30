using CurrencyTrading.DAL.DTO;
using System.Xml.Linq;

namespace CurrencyTrading.Client.Interfaces
{
    public interface ICustomMapper
    {
        ICollection<CurrencyDTO> CurrencyXmlToDto(XElement xElement);
    }
}
