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
            StockPriceResult priceResult = await stockQuoteClient.GetCurrentPriceAsync(ticker);
            if (priceResult.Success)
                {
                    Console.WriteLine($"Current price for {ticker}: {priceResult.Price}");
                }
            else
            {
                Console.WriteLine($"Failed to fetch price for {ticker}: {priceResult.ErrorMessage}");
            }

            await Task.Delay(2 * 1000);

        }
    }
}
