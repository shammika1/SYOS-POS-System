using Moq;
using SYOSSytem.Command;
using SYOSSytem.DataGateway;
using SYOSSytem.DTO;
using SYOSSytem.Facade;
using SYOSSytem.Factory;
using SYOSSytem.State;

namespace TestProject.BillingSystem;

[TestClass]
public class BillingSystemBehavior_WithMocking
{
    private BillingFacade billingFacade;
    private InventoryFacade inventoryFacade;
    private Mock<BillGateway> mockBillGateway;
    private Mock<BillItemGateway> mockBillItemGateway;
    private Mock<ItemGateway> mockItemGateway;
    private Mock<ShelfGateway> mockShelfGateway;
    private Mock<StockGateway> mockStockGateway;

    [TestInitialize]
    public void Setup()
    {
        // Initialize mocks
        mockItemGateway = new Mock<ItemGateway>();
        mockStockGateway = new Mock<StockGateway>();
        mockShelfGateway = new Mock<ShelfGateway>();
        mockBillGateway = new Mock<BillGateway>();
        mockBillItemGateway = new Mock<BillItemGateway>();

        // Set up InventoryFacade with mocked gateways
        inventoryFacade = new InventoryFacade();
        inventoryFacade.Configure(mockItemGateway.Object, mockStockGateway.Object, mockShelfGateway.Object);

        // Set up BillingFacade with mocked gateways and InventoryFacade
        billingFacade = new BillingFacade();
        billingFacade.Configure(inventoryFacade, mockBillGateway.Object, mockBillItemGateway.Object);
    }

    [TestMethod]
    public void TestProcessSaleWithMocking()
    {
        var item1 = DTOFactory.CreateItemDTO("001", "Item1", 10.00m);
        var item2 = DTOFactory.CreateItemDTO("002", "Item2", 15.00m);

        mockItemGateway.Setup(g => g.GetItem("001")).Returns(item1);
        mockItemGateway.Setup(g => g.GetItem("002")).Returns(item2);

        var billItems = new List<BillItemDTO>
        {
            DTOFactory.CreateBillItemDTO(null, "001", "Item1", 2, 20.00m), // 2 * $10
            DTOFactory.CreateBillItemDTO(null, "002", "Item2", 1, 15.00m) // 1 * $15
        };

        var context = new BillingContext(inventoryFacade, mockBillGateway.Object, mockBillItemGateway.Object)
        {
            BillItems = billItems,
            Discount = 5.00m,
            CashTendered = 50.00m
        };

        ICommand processSaleCommand = new ProcessSaleCommand(context);
        processSaleCommand.Execute();

        mockBillGateway.Verify(g => g.AddBill(It.IsAny<BillDTO>()), Times.Once);
        mockBillItemGateway.Verify(g => g.AddBillItem(It.IsAny<BillItemDTO>()), Times.Exactly(billItems.Count));

        var expectedTotalPrice = 20.00m + 15.00m; // $35
        var expectedDiscountedPrice = expectedTotalPrice - 5.00m; // $30
        var expectedChangeAmount = 50.00m - expectedDiscountedPrice; // $20

        Assert.AreEqual(expectedTotalPrice, context.BillItems.Sum(item => item.TotalPrice));
        Assert.AreEqual(expectedDiscountedPrice, expectedTotalPrice - context.Discount);
        Assert.AreEqual(expectedChangeAmount, context.CashTendered - (expectedTotalPrice - context.Discount));
    }
}