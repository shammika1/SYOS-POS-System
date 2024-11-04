using Moq;
using SYOSSytem.DataGateway;
using SYOSSytem.DTO;
using SYOSSytem.Facade;
using SYOSSytem.Factory;

namespace TestProject.InventorySystem;

[TestClass]
public class InventorySystemBehaviorTests_WithMocking
{
    private InventoryFacade inventoryFacade;
    private Mock<ItemGateway> itemGatewayMock;
    private Mock<ShelfGateway> shelfGatewayMock;
    private Mock<StockGateway> stockGatewayMock;


    [TestInitialize]
    public void Setup()
    {
        itemGatewayMock = new Mock<ItemGateway>();
        stockGatewayMock = new Mock<StockGateway>();
        shelfGatewayMock = new Mock<ShelfGateway>();

        inventoryFacade = new InventoryFacade();
        inventoryFacade.Configure(itemGatewayMock.Object, stockGatewayMock.Object, shelfGatewayMock.Object);
    }

    [TestMethod]
    public void TestAddItemWithMock()
    {
        var item = DTOFactory.CreateItemDTO("I001", "Pen", 10.5m);
        itemGatewayMock.Setup(gateway => gateway.AddItem(item));

        inventoryFacade.AddItem(item);

        itemGatewayMock.Verify(gateway => gateway.AddItem(item), Times.Once);
    }


    [TestMethod]
    public void TestEditItemWithMocking()
    {
        var item = DTOFactory.CreateItemDTO("I002", "Pencil", 5.5m);
        itemGatewayMock.Setup(gateway => gateway.EditItem(item));

        inventoryFacade.EditItem(item);

        itemGatewayMock.Verify(gateway => gateway.EditItem(item), Times.Once);
    }


    [TestMethod]
    public void TestDeleteItemWithMocking()
    {
        const string itemCode = "I003";
        itemGatewayMock.Setup(gateway => gateway.DeleteItem(itemCode));

        inventoryFacade.DeleteItem(itemCode);

        itemGatewayMock.Verify(gateway => gateway.DeleteItem(itemCode), Times.Once);
    }


    [TestMethod]
    public void TestAddStockWithMocking()
    {
        var stock = DTOFactory.CreateStockDTO(0, "I001", 100, DateTime.Now.AddMonths(1), DateTime.Now);
        stockGatewayMock.Setup(gateway => gateway.AddStock(stock));

        inventoryFacade.AddStock(stock);

        stockGatewayMock.Verify(gateway => gateway.AddStock(stock), Times.Once);
    }


    [TestMethod]
    public void TestDeleteStockWithMocking()
    {
        const int stockID = 1;
        stockGatewayMock.Setup(gateway => gateway.DeleteStock(stockID));

        inventoryFacade.DeleteStock(stockID);

        stockGatewayMock.Verify(gateway => gateway.DeleteStock(stockID), Times.Once);
    }

    [TestMethod]
    public void TestAddShelfWithMocking()
    {
        var item = DTOFactory.CreateItemDTO("ITEM001", "Pen", 10.5m);
        inventoryFacade.AddItem(item);

        var newShelf = new ShelfDTO
        {
            ShelfLocation = "A1",
            ShelfQuantity = 50,
            ItemCode = "ITEM001"
        };

        inventoryFacade.AddShelf(newShelf);

        shelfGatewayMock.Verify(g => g.AddShelf(It.Is<ShelfDTO>(s =>
            s.ShelfLocation == newShelf.ShelfLocation &&
            s.ShelfQuantity == newShelf.ShelfQuantity &&
            s.ItemCode == newShelf.ItemCode)), Times.Once);
    }


    [TestMethod]
    public void TestDeleteShelfWithMocking()
    {
        const int shelfID = 1;
        shelfGatewayMock.Setup(gateway => gateway.DeleteShelf(shelfID));

        inventoryFacade.DeleteShelf(shelfID);

        shelfGatewayMock.Verify(gateway => gateway.DeleteShelf(shelfID), Times.Once);
    }

    [TestMethod]
    public void GetAllShelves()
    {
        var shelves = new List<ShelfDTO>
        {
            new() { ShelfID = 1, ShelfLocation = "A1", ShelfQuantity = 50, ItemCode = "ITEM123" },
            new() { ShelfID = 2, ShelfLocation = "B2", ShelfQuantity = 30, ItemCode = "ITEM456" }
        };

        shelfGatewayMock.Setup(m => m.GetAllShelves()).Returns(shelves);

        var result = inventoryFacade.GetAllShelves();

        shelfGatewayMock.Verify(m => m.GetAllShelves(), Times.Once);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("A1", result[0].ShelfLocation);
    }


    [TestMethod]
    public void TestAssignItemsToShelfWithMocking()
    {
        var itemCode = "TEST123";
        var item = new ItemDTO { ItemCode = "TEST123", Name = "Test Item", Price = 10.99m };
        var stock = new StockDTO
            { StockID = 1, ItemCode = "TEST123", Quantity = 100, ExpiryDate = DateTime.Now.AddMonths(6) };
        var shelf = new ShelfDTO { ShelfID = 1, ShelfLocation = "A1", ShelfQuantity = 0, ItemCode = itemCode };

        stockGatewayMock.Setup(m => m.GetStocksByItemCode(itemCode)).Returns(new List<StockDTO> { stock });
        shelfGatewayMock.Setup(m => m.GetShelfById(shelf.ShelfID)).Returns(shelf);
        shelfGatewayMock.Setup(m => m.UpdateShelf(It.IsAny<ShelfDTO>()));

        inventoryFacade.AssignItemsToShelf(shelf.ShelfID, itemCode, 50);

        stockGatewayMock.Verify(
            m => m.UpdateStock(It.Is<StockDTO>(s => s.StockID == stock.StockID && s.Quantity == 50)), Times.Once);
        shelfGatewayMock.Verify(
            m => m.UpdateShelf(It.Is<ShelfDTO>(s => s.ShelfID == shelf.ShelfID && s.ShelfQuantity == 50)), Times.Once);
    }

    [TestMethod]
    public void TestAddNewItemDoesNotCallEditItem()
    {
        var item = DTOFactory.CreateItemDTO("I001", "Pen", 10.5m);
        itemGatewayMock.Setup(gateway => gateway.EditItem(It.IsAny<ItemDTO>()));

        inventoryFacade.AddItem(item);

        itemGatewayMock.Verify(gateway => gateway.EditItem(It.IsAny<ItemDTO>()), Times.Never);
    }

    [TestMethod]
    public void TestAddNewItemDoesNotCallDeleteItem()
    {
        var item = DTOFactory.CreateItemDTO("I002", "Pencil", 5.5m);
        itemGatewayMock.Setup(gateway => gateway.DeleteItem(It.IsAny<string>()));

        inventoryFacade.AddItem(item);

        itemGatewayMock.Verify(gateway => gateway.DeleteItem(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void TestAddStockDoesNotCallUpdateStock()
    {
        var stock = DTOFactory.CreateStockDTO(0, "I002", 50, DateTime.Now.AddMonths(2), DateTime.Now);
        stockGatewayMock.Setup(gateway => gateway.UpdateStock(It.IsAny<StockDTO>()));

        inventoryFacade.AddStock(stock);

        stockGatewayMock.Verify(gateway => gateway.UpdateStock(It.IsAny<StockDTO>()), Times.Never);
    }

    [TestMethod]
    public void TestAddStockDoesNotCallDeleteStock()
    {
        var stock = DTOFactory.CreateStockDTO(0, "I001", 100, DateTime.Now.AddMonths(1), DateTime.Now);
        stockGatewayMock.Setup(gateway => gateway.DeleteStock(It.IsAny<int>()));

        inventoryFacade.AddStock(stock);

        stockGatewayMock.Verify(gateway => gateway.DeleteStock(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public void TestAddShelfDoesNotCallDeleteShelf()
    {
        var shelf = DTOFactory.CreateShelfDTO(0, "A1", 100, "I001");
        shelfGatewayMock.Setup(gateway => gateway.DeleteShelf(It.IsAny<int>()));

        inventoryFacade.AddShelf(shelf);

        shelfGatewayMock.Verify(gateway => gateway.DeleteShelf(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public void TestAddShelfDoesNotCallUpdateShelf()
    {
        var shelf = DTOFactory.CreateShelfDTO(0, "A2", 50, "I002");
        shelfGatewayMock.Setup(gateway => gateway.UpdateShelf(It.IsAny<ShelfDTO>()));

        inventoryFacade.AddShelf(shelf);

        shelfGatewayMock.Verify(gateway => gateway.UpdateShelf(It.IsAny<ShelfDTO>()), Times.Never);
    }
}