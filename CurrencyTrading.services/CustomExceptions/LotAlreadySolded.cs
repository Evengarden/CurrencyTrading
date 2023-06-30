namespace CurrencyTrading.services.CustomExceptions
{
    public class LotAlreadySolded : InvalidOperationException
    {
        public override string Message =>
            $"Error. Lot already solded";
    }
}
