using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SYOS.Server.DataGateway;
using SYOS.Server.Services;
using SYOS.Shared.DTO;
using SYOS.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SYOS.Tests
{
    [TestClass]
    public class ReportServiceTests
    {
        private Mock<ItemGateway> _mockItemGateway;
        private Mock<StockGateway> _mockStockGateway;
        private Mock<ShelfGateway> _mockShelfGateway;
        private Mock<BillGateway> _mockBillGateway;
        private Mock<BillItemGateway> _mockBillItemGateway;
        private IReportService _reportService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockItemGateway = new Mock<ItemGateway>();
            _mockStockGateway = new Mock<StockGateway>();
            _mockShelfGateway = new Mock<ShelfGateway>();
            _mockBillGateway = new Mock<BillGateway>();
            _mockBillItemGateway = new Mock<BillItemGateway>();
            _reportService = new ReportService(
                _mockItemGateway.Object,
                _mockStockGateway.Object,
                _mockShelfGateway.Object,
                _mockBillGateway.Object,
                _mockBillItemGateway.Object);
        }

        [TestMethod]
        public async Task GetDailySaleReportAsync_ShouldReturnCorrectReport()
        {
            // Arrange
            var testDate = new DateTime(2023, 1, 1);
            var testBills = new List<BillDTO>
            {
                new BillDTO { BillID = "B001", Date = testDate, TotalPrice = 100m }
            };
            var testBillItems = new List<BillItemDTO>
            {
                new BillItemDTO { BillID = "B001", ItemCode = "I001", ItemName = "Item 1", Quantity = 2, TotalPrice = 100m }
            };

            _mockBillGateway.Setup(g => g.GetBillsByDateAsync(testDate)).ReturnsAsync(testBills);
            _mockBillItemGateway.Setup(g => g.GetBillItemsByBillIdAsync("B001")).ReturnsAsync(testBillItems);

            // Act
            var result = await _reportService.GetDailySaleReportAsync(testDate);

            // Assert
            Assert.AreEqual(testDate, result.Date);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(100m, result.TotalRevenue);
            Assert.AreEqual("Item 1", result.Items[0].ItemName);
            Assert.AreEqual(2, result.Items[0].TotalQuantity);
            Assert.AreEqual(100m, result.Items[0].TotalRevenue);
        }

    }
}