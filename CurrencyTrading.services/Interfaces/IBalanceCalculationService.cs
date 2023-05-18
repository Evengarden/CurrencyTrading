﻿using CurrencyTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyTrading.services.Interfaces
{
    public interface IBalanceCalculationService
    {
        public void CheckEnoughBalanceForSold(User user, Lot lot);
        public void CheckEnoughBalanceForBuy(User user, Lot lot);
        public Task<User> CalculateBalance(User buyer,User lotOwner, Lot lot);
    }
}
