
namespace StockQuoteAlert.DataModels
{
    /// <summary>
    /// Represents the result of parsing command-line arguments.
    /// </summary>
    public class ArgsParseResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Ticker { get; set; }
        public decimal SellPrice { get; set; }
        public decimal BuyPrice { get; set; }
    }
}