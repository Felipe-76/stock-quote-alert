using YahooFinanceApi;
using StockQuoteAlert.DataModels;

namespace StockQuoteAlert.ApiClients
{
    /// <summary>
    /// Client for retrieving stock quotes from Yahoo Finance.
    /// </summary>
    public class YahooStockQuoteClient : IStockQuoteClient
    {
        private static readonly Lazy<YahooStockQuoteClient> _instance = new(() => new YahooStockQuoteClient());
        /// <summary>
        /// Singleton instance of YahooStockQuoteClient.
        /// </summary>
        public static YahooStockQuoteClient Instance => _instance.Value;
        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private YahooStockQuoteClient() { }

        /// <summary>
        /// Gets the current price for the specified stock ticker.
        /// </summary>
        /// <param name="ticker">The stock ticker symbol.</param>
        /// <returns>A <see cref="StockPriceResult"/> containing the price and status.</returns>
        public async Task<StockPriceResult> GetCurrentPriceAsync(string ticker)
        {
            try
            {
                var securities = await Yahoo.Symbols(ticker).Fields(Field.RegularMarketPrice).QueryAsync();
                if (securities.TryGetValue(ticker, out var security) && security != null)
                {
                    var priceObj = security[Field.RegularMarketPrice];
                    if (priceObj != null)
                        return new StockPriceResult { Success = true, Price = Convert.ToDecimal(priceObj), ErrorMessage = null };
                }
                return new StockPriceResult { Success = false, Price = null, ErrorMessage = $"Price not found for ticker {ticker}" };
            }
            catch (Exception ex)
            {
                return new StockPriceResult { Success = false, Price = null, ErrorMessage = ex.Message };
            }
        }
    }
} 