namespace Wallet.NET.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = null!;
        public string Exchange { get; set; } = null!;
        public static bool IsValidExchange(string exchange)
        {
            if (string.IsNullOrEmpty(exchange))
            {
                return false;
            }

            var validExchanges = new List<string>() { ExchangeTypes.BOVESPA, ExchangeTypes.NASDAQ };

            return validExchanges.Contains(exchange.ToUpperInvariant());
        }
        public static bool IsExchangeValid(string exchange)
        {
            var validExchanges = new List<string>() { ExchangeTypes.BOVESPA, ExchangeTypes.NASDAQ };
            return validExchanges.Contains(exchange.ToUpperInvariant());
        }
    }
    public static class ExchangeTypes
    {
        public static string BOVESPA = "BOVESPA";
        public static string NASDAQ = "NASDAQ";
    }
}

