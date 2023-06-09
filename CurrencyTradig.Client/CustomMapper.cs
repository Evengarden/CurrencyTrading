﻿using CurrencyTrading.Client.Interfaces;
using CurrencyTrading.DAL.DTO;
using System.Xml.Linq;

namespace CurrencyTrading.Client
{
    public class CustomMapper : ICustomMapper
    {
        public ICollection<CurrencyDTO> CurrencyXmlToDto(XElement xElement)
        {
            List<CurrencyDTO> currencyDTOs = new List<CurrencyDTO>();

            foreach (XElement element in xElement.Elements())
            {
                currencyDTOs.Add(new CurrencyDTO
                {
                    CurrencyCode = element.Element("CharCode")!.Value,
                    CurrencyNominal = int.Parse(element.Element("Nominal")!.Value),
                    CurrencyPrice = Convert.ToDecimal(element.Element("Value")!.Value.Replace(",", "."))

                });
            }

            return currencyDTOs;
        }
    }
}
