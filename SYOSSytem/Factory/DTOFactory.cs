using SYOSSytem.DTO;

namespace SYOSSytem.Factory;

public static class DTOFactory
{
    public static ItemDTO CreateItemDTO(string itemCode, string name, decimal price)
    {
        return new ItemDTO { ItemCode = itemCode, Name = name, Price = price };
    }

    public static StockDTO CreateStockDTO(int stockID, string itemCode, int quantity, DateTime? expiryDate,
        DateTime DateOfPurchase)
    {
        return new StockDTO { StockID = stockID, ItemCode = itemCode, Quantity = quantity, ExpiryDate = expiryDate };
    }

    public static ShelfDTO CreateShelfDTO(int shelfID, string shelfLocation, int shelfQuantity, string itemID)
    {
        return new ShelfDTO
            { ShelfID = shelfID, ShelfLocation = shelfLocation, ShelfQuantity = shelfQuantity, ItemCode = itemID };
    }

    public static BillDTO CreateBillDTO(string billID, DateTime date, decimal totalPrice, decimal discount,
        decimal cashTendered, decimal changeAmount)
    {
        return new BillDTO
        {
            BillID = billID, Date = date, TotalPrice = totalPrice, Discount = discount, CashTendered = cashTendered,
            ChangeAmount = changeAmount
        };
    }

    public static BillItemDTO CreateBillItemDTO(string billID, string itemCode, string itemName, int quantity,
        decimal totalPrice)
    {
        return new BillItemDTO
            { BillID = billID, ItemCode = itemCode, ItemName = itemName, Quantity = quantity, TotalPrice = totalPrice };
    }
}