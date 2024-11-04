using SYOSSytem.Command;
using SYOSSytem.Composite;
using SYOSSytem.DataGateway;
using SYOSSytem.DTO;

namespace SYOSSytem.Facade;

public class InventoryFacade
{
    private ItemGateway itemGateway;
    private ShelfGateway shelfGateway;
    private StockGateway stockGateway;

    public InventoryFacade()
    {
        itemGateway = new ItemGateway();
        stockGateway = new StockGateway();
        shelfGateway = new ShelfGateway();
    }

    public void Configure(ItemGateway itemGateway, StockGateway stockGateway, ShelfGateway shelfGateway)
    {
        this.itemGateway = itemGateway;
        this.stockGateway = stockGateway;
        this.shelfGateway = shelfGateway;
    }

    public void AddItem(ItemDTO item)
    {
        var command = new AddItemCommand(item, itemGateway);
        command.Execute();
    }

    public void EditItem(ItemDTO item)
    {
        var command = new EditItemCommand(item, itemGateway);
        command.Execute();
    }

    public void DeleteItem(string itemCode)
    {
        var command = new DeleteItemCommand(itemCode, itemGateway);
        command.Execute();
    }

    public ItemDTO GetItem(string itemCode)
    {
        return itemGateway.GetItem(itemCode);
    }

    public List<ItemDTO> GetAllItems()
    {
        return itemGateway.GetAllItems();
    }

    public void AddStock(StockDTO stock)
    {
        var command = new AddStockCommand(stock, stockGateway);
        command.Execute();
    }

    public void DeleteStock(int stockID)
    {
        var command = new DeleteStockCommand(stockID, stockGateway);
        command.Execute();
    }

    public List<StockDTO> GetAllStocks()
    {
        return stockGateway.GetAllStocks();
    }

    public void AddShelf(ShelfDTO shelf)
    {
        var command = new AddShelfCommand(shelf, shelfGateway);
        command.Execute();
    }

    public void DeleteShelf(int shelfID)
    {
        var command = new DeleteShelfCommand(shelfID, shelfGateway);
        command.Execute();
    }

    public List<ShelfDTO> GetAllShelves()
    {
        return shelfGateway.GetAllShelves();
    }

    public void AssignItemsToShelf(int shelfID, string itemCode, int quantity)
    {
        var stocks = stockGateway.GetStocksByItemCode(itemCode);
        var shelf = shelfGateway.GetShelfById(shelfID);

        if (shelf == null)
        {
            Console.WriteLine("Shelf not found.");
            return;
        }

        if (stocks.Count == 0)
        {
            Console.WriteLine("No stock available for the given item.");
            return;
        }

        stocks = stocks.OrderByDescending(s => s.ExpiryDate).ToList();
        var remainingQuantity = quantity;

        foreach (var stock in stocks)
        {
            if (remainingQuantity == 0) break;

            var quantityToAssign = Math.Min(stock.Quantity, remainingQuantity);
            stock.Quantity -= quantityToAssign;
            remainingQuantity -= quantityToAssign;

            stockGateway.UpdateStock(stock);
        }

        if (remainingQuantity > 0)
        {
            Console.WriteLine("Not enough stock to fulfill the requested quantity.");
        }
        else
        {
            shelf.ShelfQuantity += quantity;
            shelfGateway.UpdateShelf(shelf);
            Console.WriteLine("Items assigned to shelf successfully.");
        }
    }

    public virtual void UpdateShelfQuantitiesAfterSale(List<BillItemDTO> billItems)
    {
        foreach (var billItem in billItems)
        {
            var shelves = shelfGateway.GetShelvesByItemCode(billItem.ItemCode);
            var remainingQuantity = billItem.Quantity;

            foreach (var shelf in shelves)
            {
                if (remainingQuantity == 0) break;

                var quantityToReduce = Math.Min(shelf.ShelfQuantity, remainingQuantity);
                shelf.ShelfQuantity -= quantityToReduce;
                remainingQuantity -= quantityToReduce;

                shelfGateway.UpdateShelf(shelf);
            }

            if (remainingQuantity > 0)
                Console.WriteLine(
                    $"Warning: Not enough quantity in shelves to fully cover the sale for item {billItem.ItemCode}");
        }
    }

    public void DisplayItemsUnderCategory(CategoryComposite rootCategory)
    {
        rootCategory.Display(1);
    }

    public ShelfDTO GetShelf(int shelfID)
    {
        return shelfGateway.GetShelfById(shelfID);
    }
}