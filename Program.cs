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
            
            // Parsed args
            string ticker = parseResult.Ticker ?? "";
            decimal sellPrice = parseResult.SellPrice;
            decimal buyPrice = parseResult.BuyPrice;

            // Initializing Services
            var stockQuoteClient = YahooStockQuoteClient.Instance;
            var stockMonitorService = new StockMonitorService(stockQuoteClient);
            var recipientsService = new CsvRecipientsService(csvFilePath:"email_recipients.csv", separator:",");

            List<string> recipients = recipientsService.GetRecipients();
            Console.WriteLine(string.Join(", ", recipients));

            while (true)
            {
                (StockAlertType alertType, StockPriceResult priceResult) = await stockMonitorService.CheckAlertAsync(ticker, sellPrice, buyPrice);
                Console.WriteLine(alertType);

                // TODO: 2. Read SMTP server credentials from config file.
                // TODO: 3. Send emails with SMTP.


                await Task.Delay(2 * 1000);    
            }
            
        }
    }
}
