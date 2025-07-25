using StockQuoteAlert.ApiClients;
using StockQuoteAlert.DataModels;

namespace StockQuoteAlert.Services
{
    /// <summary>
    /// Service for monitoring stock prices and determining alert conditions.
    /// </summary>
    public class StockMonitorService : IStockMonitorService
    {
        private readonly IStockQuoteClient _stockQuoteClient;
        /// <summary>
        /// Initializes a new instance of the <see cref="StockMonitorService"/> class.
        /// </summary>
        /// <param name="stockQuoteClient">Client to retrieve stock quotes.</param>
        public StockMonitorService(IStockQuoteClient stockQuoteClient)
        {
            _stockQuoteClient = stockQuoteClient;
        }

        /// <summary>
        /// Checks if the current stock price triggers a buy or sell alert.
        /// </summary>
        /// <param name="ticker">Stock ticker symbol.</param>
        /// <param name="sellPrice">Sell price threshold.</param>
        /// <param name="buyPrice">Buy price threshold.</param>
        /// <returns>A tuple containing the alert type and price result.</returns>
        public async Task<(StockAlertType alertType, StockPriceResult priceResult)> CheckAlertAsync(string ticker, decimal sellPrice, decimal buyPrice)
        {
            var priceResult = await _stockQuoteClient.GetCurrentPriceAsync(ticker);
            Console.WriteLine($"priceResult: {{ Success = {priceResult.Success}, Price = {priceResult.Price}, ErrorMessage = {priceResult.ErrorMessage} }}");

            if (!priceResult.Success || !priceResult.Price.HasValue)
                return (StockAlertType.None, priceResult);
            if (priceResult.Price.Value > sellPrice)
                return (StockAlertType.Sell, priceResult);
            if (priceResult.Price.Value < buyPrice)
                return (StockAlertType.Buy, priceResult);
            return (StockAlertType.None, priceResult);
        }
    }
} 