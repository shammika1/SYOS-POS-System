using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SYOS.Server.DataGateway;
using SYOS.Server.Services;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SYOS.Server.Hubs;

namespace SYOS.Tests
{
    [TestClass]
    public class StockServiceTests
    {
        private Mock<StockGateway> _mockStockGateway;
        private Mock<IHubContext<SYOSHub>> _mockHubContext;
        private IStockService _stockService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockStockGateway = new Mock<StockGateway>();
            _mockHubContext = new Mock<IHubContext<SYOSHub>>();
            _stockService = new StockService(_mockStockGateway.Object, _mockHubContext.Object);
        }

        [TestMethod]
        public async Task GetAllStocksAsync_ShouldReturnAllStocks()
        {
            // Arrange
            var expectedStocks = new List<StockDTO>
            {
                new StockDTO { StockID = 1, ItemCode = "I001", Quantity = 100 },
                new StockDTO { StockID = 2, ItemCode = "I002", Quantity = 200 }
            };
            _mockStockGateway.Setup(g => g.GetAllStocksAsync()).ReturnsAsync(expectedStocks);

            // Act
            var result = await _stockService.GetAllStocksAsync();

            // Assert
            CollectionAssert.AreEqual(expectedStocks, result);
        }

        [TestMethod]
        public async Task UpdateStockAsync_ShouldUpdateStockAndBroadcast()
        {
            // Arrange
            var updatedStock = new StockDTO { StockID = 1, ItemCode = "I001", Quantity = 150 };
            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            _mockHubContext.Setup(h => h.Clients.All).Returns(mockClientProxy.Object);

            // Act
            await _stockService.UpdateStockAsync(updatedStock);

            // Assert
            _mockStockGateway.Verify(g => g.UpdateStockAsync(updatedStock), Times.Once);
            _mockHubContext.Verify(h => h.Clients.All.SendAsync(
                    It.Is<string>(methodName => methodName == "ReceiveStockUpdate"),
                    It.Is<object>(arg => arg == updatedStock),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}