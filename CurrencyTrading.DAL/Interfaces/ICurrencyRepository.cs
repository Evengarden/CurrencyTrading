﻿using CurrencyTrading.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.DAL.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<string?> GetCurrency(string code);

        Task SetCurrency(string currencyCode, string currencyJson);

        Task SetCurrencyCodes(List<string> currencies);

        Task<string?> GetCurrencyCodes();
    }
}
