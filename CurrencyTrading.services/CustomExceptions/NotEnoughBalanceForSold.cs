namespace CurrencyTrading.services.CustomExceptions
{
    public class NotEnoughBalanceForSold : InvalidOperationException
    {
        public decimal CurrentBalance { get; init; }
        public decimal LotCurrencyAmount { get; init; }
        public decimal UserLotAmountSum { get; init; }
        public override string Message => 
            $"Cannot create lot. User balance is not enough. User balance:{CurrentBalance} , " +
            $"lot amount:{LotCurrencyAmount}, total amount of user sold lots: {UserLotAmountSum}";
    }
}
