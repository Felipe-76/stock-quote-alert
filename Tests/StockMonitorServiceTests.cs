using Moq;
using StockQuoteAlert.ApiClients;
using StockQuoteAlert.DataModels;
using StockQuoteAlert.Services;
using Xunit;

namespace StockQuoteAlert.Tests
{
    public class StockMonitorServiceTests
    {
        [Theory]
        [InlineData(30, 20, 20, StockAlertType.Sell)]  // Should pass because currentPrice is greater than sellPrice.
        [InlineData(10, 20, 20, StockAlertType.Buy)]  // Should pass because currentPrice is less than buyPrice.
        [InlineData(25, 30, 20, StockAlertType.None)]  // Should pass because currentPrice is between sellPrice and buyPrice.
        
        public async Task CheckAlertAsync_ReturnsExpectedAlertType(decimal currentPrice, decimal sellPrice, decimal buyPrice, StockAlertType expected)
        {
            var mockClient = new Mock<IStockQuoteClient>();
            mockClient.Setup(x => x.GetCurrentPriceAsync(It.IsAny<string>()))
                .ReturnsAsync(new StockPriceResult { Success = true, Price = currentPrice, ErrorMessage = null });
            var service = new StockMonitorService(mockClient.Object);

            (StockAlertType alertType, StockPriceResult priceResult) = await service.CheckAlertAsync("TEST", sellPrice, buyPrice);

            Assert.Equal(expected, alertType);
        }
    }
} 