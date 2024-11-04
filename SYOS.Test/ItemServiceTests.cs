using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SYOS.Server.DataGateway;
using SYOS.Server.Hubs;
using SYOS.Server.Services;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;

namespace SYOS.Test
{
    [TestClass]
    public class ItemServiceTests
    {
        private Mock<ItemGateway> _mockItemGateway;
        private Mock<IHubContext<SYOSHub>> _mockHubContext;
        private IItemService _itemService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockItemGateway = new Mock<ItemGateway>();
            _mockHubContext = new Mock<IHubContext<SYOSHub>>();
            _itemService = new ItemService(_mockItemGateway.Object, _mockHubContext.Object);
        }

        [TestMethod]
        public async Task GetAllItemsAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expectedItems = new List<ItemDTO>
            {
                new ItemDTO { ItemCode = "I001", Name = "Item 1", Price = 10.0m },
                new ItemDTO { ItemCode = "I002", Name = "Item 2", Price = 20.0m }
            };
            _mockItemGateway.Setup(g => g.GetAllItemsAsync()).ReturnsAsync(expectedItems);

            // Act
            var result = await _itemService.GetAllItemsAsync();

            // Assert
            CollectionAssert.AreEqual(expectedItems, result);
        }

        [TestMethod]
        public async Task AddItemAsync_ShouldAddItemAndBroadcast()
        {
            // Arrange
            var newItem = new ItemDTO { ItemCode = "I003", Name = "Item 3", Price = 30.0m };
            var mockClients = new Mock<IHubClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            _mockHubContext.Setup(h => h.Clients.All).Returns(mockClientProxy.Object);

            // Act
            await _itemService.AddItemAsync(newItem);

            // Assert
            _mockItemGateway.Verify(g => g.AddItemAsync(newItem), Times.Once);
            _mockHubContext.Verify(h => h.Clients.All.SendAsync(
                    It.Is<string>(methodName => methodName == "ReceiveItemUpdate"),
                    It.Is<object>(arg => arg == newItem),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }



    }
}
