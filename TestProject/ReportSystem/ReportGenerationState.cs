using Moq;
using SYOSSytem.DataGateway;
using SYOSSytem.Facade;
using SYOSSytem.Factory;
using SYOSSytem.Template;

namespace TestProject.ReportSystem;

[TestClass]
public class ReportGenerationState
{
    private Report billReport;

    private Report dailySalesReport;
    private InventoryFacade inventoryFacade;
    private Report reorderReport;
    private ReportGateway reportGateway;
    private Mock<ReportGateway> reportGatewayMock;
    private Report reshelvingReport;
    private Report stockReport;

    [TestInitialize]
    public void Setup()
    {
        DatabaseCleaner.Clean();
        inventoryFacade = new InventoryFacade();
        reportGateway = new ReportGateway();

        reportGatewayMock = new Mock<ReportGateway>();
        dailySalesReport = new DailySalesReport(DateTime.Now);
        reshelvingReport = new ReshelvingReport();
        reorderReport = new ReorderReport();
        stockReport = new StockReport();
        billReport = new BillReport();

        // Assigning test data
        var item1 = DTOFactory.CreateItemDTO("I001", "Pen", 10.5m);
        var item2 = DTOFactory.CreateItemDTO("I002", "Pencil", 5.5m);
        inventoryFacade.AddItem(item1);
        inventoryFacade.AddItem(item2);

        var stock1 = DTOFactory.CreateStockDTO(0, "I001", 100, DateTime.Now.AddMonths(1), DateTime.Now);
        var stock2 = DTOFactory.CreateStockDTO(0, "I002", 200, DateTime.Now.AddMonths(2), DateTime.Now);
    }

    [TestMethod]
    public void TestGenerateDailySalesReport()
    {
        var reportDate = DateTime.Now;

        var report = new DailySalesReport(reportDate);

        report.GenerateReport();

        var reportData = reportGateway.GetDailySalesReport(reportDate);
        Assert.IsNotNull(reportData);
    }

    [TestMethod]
    public void TestGenerateReshelvingReport()
    {
        var report = new ReshelvingReport();
        report.GenerateReport();

        var reportData = reportGateway.GetReshelvingReport();
        Assert.IsNotNull(reportData);
    }

    [TestMethod]
    public void TestGenerateReorderReport()
    {
        var report = new ReorderReport();
        report.GenerateReport();

        var reportData = reportGateway.GetReorderReport();
        Assert.IsNotNull(reportData);
    }

    [TestMethod]
    public void TestGenerateStockReport()
    {
        var report = new StockReport();
        report.GenerateReport();

        var reportData = reportGateway.GetStockReport();
        Assert.IsNotNull(reportData);
    }

    [TestMethod]
    public void TestGenerateBillReport()
    {
        var report = new BillReport();
        report.GenerateReport();

        var reportData = reportGateway.GetBillReport();
        Assert.IsNotNull(reportData);
    }
}