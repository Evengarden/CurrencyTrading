﻿namespace CurrencyTrading.Models
{
    public class Trade
    {
        public int Id { get; set; }
        public DateTime TradeDate { get; set; }
        public int Lot_Id { get; set; }
        public Lot TradeLot { get; set; }
        public User Buyer { get; set; }
    }
}
