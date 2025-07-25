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
        /// <summary>
        /// Filepath of smtp json settings.
        /// </summary>
        private const string SmtpSettingsFilePath = "smtpsettings.json";
        /// <summary>
        /// Filepath of recipients csv.
        /// </summary>
        private const string RecipientsCsvFilePath = "email_recipients.csv";
        /// <summary>
        /// Recipients csv columns separator.
        /// </summary>
        private const string CsvSeparator = ",";
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
            var mailService = new SmtpEmailService(configFilePath: SmtpSettingsFilePath);
            var recipientsService = new CsvRecipientsService(csvFilePath: RecipientsCsvFilePath, separator: CsvSeparator);

            // Extracting recipients from recipients service
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
