using StockQuoteAlert.Services;
using StockQuoteAlert.ApiClients;
using StockQuoteAlert.DataModels;

namespace StockQuoteAlert
{
    class Program
    {
        /// <summary>
        /// Interval in minutes for each stock price checking and email sending.
        /// </summary>
        private const int AlertIntervalMins = 10;
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
            var recipientsService = new CsvRecipientsService(csvFilePath: "email_recipients.csv", separator: ",");
            var mailService = new SmtpEmailService(configFilePath: "smtpsettings_test.json");

            List<string> recipients = recipientsService.GetRecipients();

            while (true)
            {
                (StockAlertType alertType, StockPriceResult priceResult) = await stockMonitorService.CheckAlertAsync(ticker, sellPrice, buyPrice);

                if (alertType != StockAlertType.None)
                {
                    string subject = $"Stock Alert: {ticker} - {alertType} Price Reached!";
                    string body = $"The stock {ticker} has reached a {alertType} price of {priceResult.Price}. " +
                                $"Sell price: {sellPrice}, Buy price: {buyPrice}.";

                    await mailService.SendAlertAsync(recipients, subject, body);
                }

                await Task.Delay(AlertIntervalMins * 60 * 1000);
            }
        }
    }
}
