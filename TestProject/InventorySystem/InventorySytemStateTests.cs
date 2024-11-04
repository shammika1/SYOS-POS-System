using Microsoft.Data.SqlClient;
using SYOSSytem.DataGateway;
using SYOSSytem.DTO;
using SYOSSytem.Facade;
using SYOSSytem.Singleton;

namespace TestProject.InventorySystem;

[TestClass]
public class InventorySytemStateTests
{
    private InventoryFacade inventoryFacade;
    private ItemGateway itemGateway;
    private ShelfGateway shelfGateway;
    private StockGateway stockGateway;

    [TestInitialize]
    public void Setup()
    {
        itemGateway = new ItemGateway();
        stockGateway = new StockGateway();
        shelfGateway = new ShelfGateway();
        inventoryFacade = new InventoryFacade();
        {
            itemGateway = new ItemGateway();
            stockGateway = new StockGateway();
            shelfGateway = new ShelfGateway();
        }
        DatabaseCleaner.Clean();
    }

    [TestMethod]
    public void TestAddItemVerifyItemAdded()
    {
        var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
        inventoryFacade.AddItem(item);

        var addedItem = itemGateway.GetItem("123");
        Assert.IsNotNull(addedItem);
        Assert.AreEqual("Test Item", addedItem.Name);
    }

    [TestMethod]
    public void TestEditItemVerifyItemEdited()
    {
        var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
        inventoryFacade.AddItem(item);

        item.Name = "Updated Test Item";
        item.Price = 60;
        inventoryFacade.EditItem(item);

        var updatedItem = itemGateway.GetItem("123");
        Assert.IsNotNull(updatedItem);
        Assert.AreEqual("Updated Test Item", updatedItem.Name);
        Assert.AreEqual(60, updatedItem.Price);
    }

    [TestMethod]
    public void TestDeleteItemVerifyItemDeleted()
    {
        var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
        inventoryFacade.AddItem(item);
        inventoryFacade.DeleteItem("123");

        var deletedItem = itemGateway.GetItem("123");
        Assert.IsNull(deletedItem);
    }

    [TestMethod]
    public void TestAddStockVerifyStockAdded()
    {
        var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
        inventoryFacade.AddItem(item);

        var stock = new StockDTO
        {
            StockID = 1, ItemCode = "123", Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6),
            DateOfPurchase = DateTime.Now
        };
        inventoryFacade.AddStock(stock);

        var addedStock = stockGateway.GetStocksByItemCode("123").FirstOrDefault();
        Assert.IsNotNull(addedStock);
        Assert.AreEqual(100, addedStock.Quantity);
    }

    [TestMethod]
    public void TestDeleteStockVerifyStockDeleted()
    {
        var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
        inventoryFacade.AddItem(item);

        var stock = new StockDTO
        {
            StockID = 1, ItemCode = "123", Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6),
            DateOfPurchase = DateTime.Now
        };
        inventoryFacade.AddStock(stock);
        inventoryFacade.DeleteStock(1);

        var deletedStock = stockGateway.GetStocksByStockID(1).FirstOrDefault();
        Assert.IsNull(deletedStock);
    }

    [TestMethod]
    public void TestAddShelfVerifyShelfAdded()
    {
        {
            var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
            inventoryFacade.AddItem(item);
            var shelfGateway = new ShelfGateway();
            var newShelf = new ShelfDTO
            {
                ShelfLocation = "TestLocation",
                ShelfQuantity = 50,
                ItemCode = "123"
            };

            shelfGateway.AddShelf(newShelf);

            using (var connection = new SqlConnection(DatabaseConnection.Instance.ConnectionString))
            {
                connection.Open();
                using (var command =
                       new SqlCommand(
                           "SELECT COUNT(*) FROM Shelf WHERE ShelfLocation = @ShelfLocation AND ShelfQuantity = @ShelfQuantity AND ItemID = @ItemID",
                           connection))
                {
                    command.Parameters.AddWithValue("@ShelfLocation", newShelf.ShelfLocation);
                    command.Parameters.AddWithValue("@ShelfQuantity", newShelf.ShelfQuantity);
                    command.Parameters.AddWithValue("@ItemID", newShelf.ItemCode);

                    var count = (int)command.ExecuteScalar();
                    Assert.AreEqual(1, count, "Shelf was not added correctly.");
                }
            }
        }
    }

    [TestMethod]
    public void TestDeleteShelfVerifyShelfDeleted()
    {
        var item = new ItemDTO { ItemCode = "123", Name = "Test Item", Price = 50 };
        inventoryFacade.AddItem(item);

        var shelf = new ShelfDTO { ShelfID = 1, ShelfLocation = "A1", ShelfQuantity = 50, ItemCode = "123" };
        inventoryFacade.AddShelf(shelf);
        inventoryFacade.DeleteShelf(1);

        var deletedShelf = shelfGateway.GetShelfById(1);
        Assert.IsNull(deletedShelf);
    }

    [TestMethod]
    public void TestAssignItemsToShelfUpdatesInventory()
    {
        var itemCode = "item001";
        var shelfID = 1;
        var quantity = 10;

        var item = new ItemDTO { ItemCode = itemCode, Name = "Test Item", Price = 100 };
        var stock = new StockDTO
        {
            StockID = 1, ItemCode = itemCode, Quantity = 20, ExpiryDate = DateTime.Now.AddDays(10),
            DateOfPurchase = DateTime.Now
        };
        var shelf = new ShelfDTO { ShelfID = shelfID, ShelfLocation = "A1", ShelfQuantity = 0, ItemCode = itemCode };

        itemGateway.AddItem(item);
        stockGateway.AddStock(stock);
        shelfGateway.AddShelf(shelf);

        inventoryFacade.AssignItemsToShelf(shelfID, itemCode, quantity);

        var updatedStock = stockGateway.GetStocksByItemCode(itemCode).Find(s => s.StockID == stock.StockID);
        var updatedShelf = shelfGateway.GetShelfById(shelfID);

        Assert.IsNotNull(updatedStock);
        Assert.AreEqual(10, updatedStock.Quantity);

        Assert.IsNotNull(updatedShelf);
        Assert.AreEqual(10, updatedShelf.ShelfQuantity);
    }
}