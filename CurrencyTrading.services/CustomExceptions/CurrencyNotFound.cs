namespace CurrencyTrading.services.CustomExceptions
{
    public class CurrencyNotFound : InvalidOperationException
    {
        public string Currency { get; init; }
        public override string Message =>
            $"Error. Non-existed currency {Currency} ";
    }
}
