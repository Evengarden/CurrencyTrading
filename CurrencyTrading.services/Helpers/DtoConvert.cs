using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurrencyTrading.services.Helpers
{
    public static class DtoConvert
    {
        public static LotDTO LotToDto(Lot lot)
        {
            return new LotDTO
            {
                Currency = lot.Currency,
                CurrencyAmount = lot.CurrencyAmount,
                Price = lot.Price,
                Type = lot.Type
            };
        }

        public static ICollection<CurrencyDTO> XmlToCurrencyDTO(XElement xml)
        {
            List<CurrencyDTO> currencyDTOs = new List<CurrencyDTO>();

            foreach (XElement element in xml.Elements())
            {
                currencyDTOs.Add(new CurrencyDTO
                {
                    CurrencyCode = element.Element("CharCode").Value,
                    CurrencyNominal = int.Parse(element.Element("Nominal").Value),
                    CurrencyPrice = Convert.ToDecimal(element.Element("Value").Value.Replace(",","."))

                });
            }

            return currencyDTOs;
        }
    }
}
