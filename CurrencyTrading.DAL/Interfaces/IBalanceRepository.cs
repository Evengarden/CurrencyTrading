﻿using CurrencyTrading.Models;

namespace CurrencyTrading.Interfaces
{
    public interface IBalanceRepository
    {
        Task<Balance> CreateBalanceAsync(Balance balance);
        Task<Balance> UpdateBalanceAsync(int balanceId,Balance balance);
        Task<Balance> DeleteBalanceAsync(int balanceId);
    }
}
