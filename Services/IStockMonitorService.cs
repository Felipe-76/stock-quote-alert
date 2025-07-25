using StockQuoteAlert.DataModels;

namespace StockQuoteAlert.Services
{
    public interface IStockMonitorService
    {
        Task<(StockAlertType alertType, StockPriceResult priceResult)> CheckAlertAsync(string ticker, decimal sellPrice, decimal buyPrice);
    }
}