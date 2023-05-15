using CurrencyTrading.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyTrading.CbClient.Interfaces
{
    public interface ICustomMapper
    {
        ICollection<CurrencyDTO> XmlToDto(XElement xElement);
    }
}
