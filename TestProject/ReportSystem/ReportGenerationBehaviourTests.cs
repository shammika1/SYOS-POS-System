using Moq;
using SYOSSytem.DataGateway;
using SYOSSytem.DTO;
using SYOSSytem.Template;

namespace TestProject.ReportSystem;

[TestClass]
public class ReportGenerationBehaviourTests
{
    private Report billReport;
    private Report dailySalesReport;
    private Report reorderReport;
    private Mock<ReportGateway> reportGatewayMock;
    private Report reshelvingReport;
    private Report stockReport;

    [TestInitialize]
    public void Setup()
    {
        reportGatewayMock = new Mock<ReportGateway>();
        dailySalesReport = new DailySalesReport(DateTime.Now);
        reshelvingReport = new ReshelvingReport();
        reorderReport = new ReorderReport();
        stockReport = new StockReport();
        billReport = new BillReport();
    }

    //[TestMethod]
    //public void TestGenerateDailySalesReportWithMocking()
    //{
    //    DateTime reportDate = DateTime.Now;

    //    DailySalesReport report = new DailySalesReport(reportDate);

    //    report.GenerateReport();
    //    reportGatewayMock.Verify(r => r.GetDailySalesReport(reportDate), Times.Once);


    //}

    //[TestMethod]
    //public void TestGenerateReshelvingReportWithMocking()
    //{
    //    var reshelvingData = new List<ReshelvingReportDTO>
    //    {
    //        new ReshelvingReportDTO { ItemCode = "I001", ItemName = "Pen", Quantity = 50 },
    //        new ReshelvingReportDTO { ItemCode = "I002", ItemName = "Pencil", Quantity = 40 }
    //    };

    //    reportGatewayMock.Setup(gateway => gateway.GetReshelvingReport()).Returns(reshelvingData);

    //    reshelvingReport.GenerateReport();

    //    reportGatewayMock.Verify(gateway => gateway.GetReshelvingReport(), Times.Once);
    //}

    //[TestMethod]
    //public void TestGenerateReorderReportWithMocking()
    //{
    //    var reorderData = new List<ReorderReportDTO>
    //    {
    //        new ReorderReportDTO { ItemCode = "I001", ItemName = "Pen", RemainingQuantity = 20 },
    //        new ReorderReportDTO { ItemCode = "I002", ItemName = "Pencil", RemainingQuantity = 30 }
    //    };

    //    reportGatewayMock.Setup(gateway => gateway.GetReorderReport()).Returns(reorderData);

    //    reorderReport.GenerateReport();

    //    reportGatewayMock.Verify(gateway => gateway.GetReorderReport(), Times.Once);
    //}

    //[TestMethod]
    //public void TestGenerateStockReportWithMocking()
    //{
    //    var stockData = new List<StockReportDTO>
    //    {
    //        new StockReportDTO { StockID = 1, ItemCode = "I001", ItemName = "Pen", Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(1) },
    //        new StockReportDTO { StockID = 2, ItemCode = "I002", ItemName = "Pencil", Quantity = 200, ExpiryDate = DateTime.Now.AddMonths(2) }
    //    };

    //    reportGatewayMock.Setup(gateway => gateway.GetStockReport()).Returns(stockData);

    //    stockReport.GenerateReport();

    //    reportGatewayMock.Verify(gateway => gateway.GetStockReport(), Times.Once);
    //}

    //[TestMethod]
    //public void TestGenerateBillReportWithMocking()
    //{
    //    var billData = new List<BillReportDTO>
    //    {
    //        new BillReportDTO { BillID = "B001", Date = DateTime.Now, TotalPrice = 150m, Discount = 10m, CashTendered = 160m, ChangeAmount = 10m },
    //        new BillReportDTO { BillID = "B002", Date = DateTime.Now, TotalPrice = 200m, Discount = 20m, CashTendered = 220m, ChangeAmount = 20m }
    //    };

    //    reportGatewayMock.Setup(gateway => gateway.GetBillReport()).Returns(billData);

    //    billReport.GenerateReport();

    //    reportGatewayMock.Verify(gateway => gateway.GetBillReport(), Times.Once);
    //}


    [TestMethod]
    public void TestGenerateDailySalesReportDoesNotCallOtherReports()
    {
        reportGatewayMock.Setup(gateway => gateway.GetDailySalesReport(It.IsAny<DateTime>()))
            .Returns(new List<SalesReportDTO>());

        dailySalesReport.GenerateReport();

        reportGatewayMock.Verify(gateway => gateway.GetReshelvingReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReorderReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetStockReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetBillReport(), Times.Never);
    }

    [TestMethod]
    public void TestGenerateReshelvingReportDoesNotCallOtherReports()
    {
        reportGatewayMock.Setup(gateway => gateway.GetReshelvingReport()).Returns(new List<ReshelvingReportDTO>());

        reshelvingReport.GenerateReport();

        reportGatewayMock.Verify(gateway => gateway.GetDailySalesReport(It.IsAny<DateTime>()), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReorderReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetStockReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetBillReport(), Times.Never);
    }

    [TestMethod]
    public void TestGenerateReorderReportDoesNotCallOtherReports()
    {
        reportGatewayMock.Setup(gateway => gateway.GetReorderReport()).Returns(new List<ReorderReportDTO>());

        reorderReport.GenerateReport();

        reportGatewayMock.Verify(gateway => gateway.GetDailySalesReport(It.IsAny<DateTime>()), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReshelvingReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetStockReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetBillReport(), Times.Never);
    }

    [TestMethod]
    public void TestGenerateStockReportDoesNotCallOtherReports()
    {
        reportGatewayMock.Setup(gateway => gateway.GetStockReport()).Returns(new List<StockReportDTO>());

        stockReport.GenerateReport();

        reportGatewayMock.Verify(gateway => gateway.GetDailySalesReport(It.IsAny<DateTime>()), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReshelvingReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReorderReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetBillReport(), Times.Never);
    }

    [TestMethod]
    public void TestGenerateBillReportDoesNotCallOtherReports()
    {
        reportGatewayMock.Setup(gateway => gateway.GetBillReport()).Returns(new List<BillReportDTO>());

        billReport.GenerateReport();

        reportGatewayMock.Verify(gateway => gateway.GetDailySalesReport(It.IsAny<DateTime>()), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReshelvingReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetReorderReport(), Times.Never);
        reportGatewayMock.Verify(gateway => gateway.GetStockReport(), Times.Never);
    }
}