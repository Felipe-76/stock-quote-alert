using StockQuoteAlert.DataModels;


namespace StockQuoteAlert.Services
{
    public static class ArgsParser
    {
        /// <summary>
        /// Parses the command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The result of parsing, including ticker, sell price, and buy price.</returns>
        public static ArgsParseResult Parse(string[] args)
        {
            if (args.Length != 3)
            {
                return new ArgsParseResult
                {
                    Success = false,
                    ErrorMessage = "Usage: dotnet run -- <TICKER> <SELL_PRICE> <BUY_PRICE>\nExample: dotnet run -- PETR4.SA 22.67 22.59"
                };
            }

            string ticker = args[0];
            if (!decimal.TryParse(args[1], out decimal sellPrice))
            {
                return new ArgsParseResult
                {
                    Success = false,
                    ErrorMessage = "Invalid sell price."
                };
            }
            if (!decimal.TryParse(args[2], out decimal buyPrice))
            {
                return new ArgsParseResult
                {
                    Success = false,
                    ErrorMessage = "Invalid buy price."
                };
            }

            return new ArgsParseResult
            {
                Success = true,
                Ticker = ticker,
                SellPrice = sellPrice,
                BuyPrice = buyPrice
            };
        }
    }

}