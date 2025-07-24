namespace StockQuoteAlert.DataModels
{
    /// <summary>
    /// Represents the result of a stock price query.
    /// </summary>
    public class StockPriceResult
    {
        public bool Success { get; set; }
        public decimal? Price { get; set; }
        public string? ErrorMessage { get; set; }
    }
}