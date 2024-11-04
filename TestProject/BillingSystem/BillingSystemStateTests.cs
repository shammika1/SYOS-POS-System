using Microsoft.Data.SqlClient;
using SYOSSytem.Command;
using SYOSSytem.DataGateway;
using SYOSSytem.DTO;
using SYOSSytem.Facade;
using SYOSSytem.Factory;
using SYOSSytem.Singleton;
using SYOSSytem.State;

namespace TestProject.BillingSystem;

[TestClass]
public class BillingSystemStateTests
{
    private BillGateway billGateway;
    private BillingContext billingContext;
    private BillItemGateway billItemGateway;
    private InventoryFacade inventoryFacade;

    [TestInitialize]
    public void Setup()
    {
        inventoryFacade = new InventoryFacade();
        billGateway = new BillGateway();
        billItemGateway = new BillItemGateway();
        billingContext = new BillingContext(inventoryFacade, billGateway, billItemGateway);
        DatabaseCleaner.Clean();
    }

    [TestMethod]
    public void TestItemEntryState()
    {
        billingContext.SetState(new ItemEntryState());

        using (var input = new StringReader("ITEM001\n2\nITEM002\n1\n00\n"))
        {
            Console.SetIn(input);
            billingContext.SetState(new ItemEntryState());
        }

        Assert.AreEqual(2, billingContext.BillItems.Count);
        Assert.AreEqual("ITEM001", billingContext.BillItems[0].ItemCode);
        Assert.AreEqual(2, billingContext.BillItems[0].Quantity);
        Assert.AreEqual("ITEM002", billingContext.BillItems[1].ItemCode);
        Assert.AreEqual(1, billingContext.BillItems[1].Quantity);

        Assert.IsInstanceOfType(billingContext.CurrentState(), typeof(PaymentState));
    }

    [TestMethod]
    public void TestPaymentState()
    {
        billingContext.SetState(new ItemEntryState());
        using (var input = new StringReader("ITEM001\n2\n00\n"))
        {
            Console.SetIn(input);
            billingContext.SetState(new ItemEntryState());
        }

        billingContext.SetState(new PaymentState());
        using (var input = new StringReader("10\n100\n"))
        {
            Console.SetIn(input);
            billingContext.SetState(new PaymentState());
        }

        Assert.AreEqual(10, billingContext.Discount);
        Assert.AreEqual(100, billingContext.CashTendered);

        Assert.IsInstanceOfType(billingContext.CurrentState(), typeof(ReceiptGenerationState));
    }

    [TestMethod]
    public void TestReceiptGenerationState()
    {
        billingContext.SetState(new ItemEntryState());
        using (var input = new StringReader("ITEM001\n2\n00\n"))
        {
            Console.SetIn(input);
            billingContext.SetState(new ItemEntryState());
        }

        billingContext.SetState(new PaymentState());
        using (var input = new StringReader("10\n100\n"))
        {
            Console.SetIn(input);
            billingContext.SetState(new PaymentState());
        }

        billingContext.SetState(new ReceiptGenerationState());

        var bills = billGateway.GetAllBills();
        Assert.IsTrue(bills.Count > 0);

        var billItems = billItemGateway.GetAllBillItems();
        Assert.IsTrue(billItems.Count > 0);

        Assert.IsInstanceOfType(billingContext.CurrentState(), typeof(ReceiptGenerationState));
    }


    [TestMethod]
    public void TestProcessSale()
    {
        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            connection.Open();

            // Insert test data
            var insertCommand = new SqlCommand(@"
                INSERT INTO Item (ItemCode, Name, Price) VALUES ('item001', 'Test Item 1', 10.0);
                INSERT INTO Stock (ItemCode, Quantity, ExpiryDate) VALUES ('item001', 100, '2025-12-31');
                INSERT INTO Shelf (ShelfLocation, ShelfQuantity, ItemID) VALUES ('A1', 50, 'item001');
            ", connection);
            insertCommand.ExecuteNonQuery();
        }

        var inventoryFacade = new InventoryFacade();
        var billGateway = new BillGateway();
        var billItemGateway = new BillItemGateway();
        var billingFacade = new BillingFacade();

        var billItems = new List<BillItemDTO>();

        var item = inventoryFacade.GetItem("item001");
        Assert.IsNotNull(item, "Item should exist");

        var billItem = DTOFactory.CreateBillItemDTO(null, item.ItemCode, item.Name, 5, item.Price * 5);
        billItems.Add(billItem);

        var discount = 2.0m;
        var cashTendered = 50.0m;

        var billingContext = new BillingContext(inventoryFacade, billGateway, billItemGateway)
        {
            BillItems = billItems,
            Discount = discount,
            CashTendered = cashTendered
        };
        ICommand processSaleCommand = new ProcessSaleCommand(billingContext);
        processSaleCommand.Execute();

        using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
        {
            connection.Open();

            // Check if bill is created
            var billCommand =
                new SqlCommand(
                    "SELECT COUNT(*) FROM Bill WHERE TotalPrice = 48.0 AND Discount = 2.0 AND CashTendered = 50.0 AND ChangeAmount = 2.0",
                    connection);
            var billCount = (int)billCommand.ExecuteScalar();
            Assert.AreEqual(1, billCount, "One bill should be created with correct details");

            // Check if bill items are created
            var billItemCommand =
                new SqlCommand(
                    "SELECT COUNT(*) FROM BillItem WHERE ItemCode = 'item001' AND Quantity = 5 AND TotalPrice = 50.0",
                    connection);
            var billItemCount = (int)billItemCommand.ExecuteScalar();
            Assert.AreEqual(1, billItemCount, "One bill item should be created with correct details");

            // Check if shelf quantity is updated
            var shelfCommand =
                new SqlCommand("SELECT ShelfQuantity FROM Shelf WHERE ItemID = 'item001' AND ShelfLocation = 'A1'",
                    connection);
            var shelfQuantity = (int)shelfCommand.ExecuteScalar();
            Assert.AreEqual(45, shelfQuantity, "Shelf quantity should be updated correctly");
        }
    }
}