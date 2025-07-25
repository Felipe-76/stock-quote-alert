using StockQuoteAlert.Services;
using StockQuoteAlert.ApiClients;
using StockQuoteAlert.DataModels;

namespace StockQuoteAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var parseResult = ArgsParser.Parse(args);
            if (!parseResult.Success)

            {
                Console.WriteLine(parseResult.ErrorMessage);
                return;
            }

            string ticker = parseResult.Ticker ?? "";
            decimal sellPrice = parseResult.SellPrice;
            decimal buyPrice = parseResult.BuyPrice;


            Console.WriteLine($"ticker: {ticker}");
            Console.WriteLine($"sellPrice: {sellPrice}");
            Console.WriteLine($"buyPrice: {buyPrice}");

            // TODO: 2. Send emails with SMTP.
            // TODO: 3. Read SMTP server credentials from config file.
            // TODO: 4. Read recipients from config file.

            var stockQuoteClient = YahooStockQuoteClient.Instance;
            var stockMonitorService = new StockMonitorService(stockQuoteClient);
            
            (StockAlertType alertType, StockPriceResult priceResult) = await stockMonitorService.CheckAlertAsync(ticker, sellPrice, buyPrice);
            
            Console.WriteLine(alertType);



            await Task.Delay(2 * 1000);

        }
    }
}
