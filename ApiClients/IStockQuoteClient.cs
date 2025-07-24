using StockQuoteAlert.DataModels;

namespace StockQuoteAlert.ApiClients
{
    /// <summary>
    /// Interface for a client that retrieves stock quotes.
    /// </summary>
    public interface IStockQuoteClient
    {
        /// <summary>
        /// Gets the current price for the specified stock ticker.
        /// </summary>
        /// <param name="ticker">The stock ticker symbol.</param>
        /// <returns>A <see cref="StockPriceResult"/> containing the price and status.</returns>
        Task<StockPriceResult> GetCurrentPriceAsync(string ticker);
    }
} 