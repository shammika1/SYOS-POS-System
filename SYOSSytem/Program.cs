using SYOSSytem.Composite;
using SYOSSytem.DTO;
using SYOSSytem.Facade;
using SYOSSytem.Template;

namespace SYOSSytem;

internal class Program
{
    private static readonly InventoryFacade inventoryFacade = new();
    private static BillingFacade billingFacade = new();
    private static ReportFacade reportFacade = new();
    private static readonly CategoryComposite rootCategory = new("Root");

    private static void Main(string[] args)
    {
        // Authenticate the user
        if (PasswordAccess.Authenticate())
        {
            Console.WriteLine("Access granted.");
            RunSystem();
        }
        else
        {
            Console.WriteLine("Access denied.");
        }
    }

    private static void RunSystem()
    {
        var running = true;

        while (running)
        {
            Console.WriteLine("\nSYOS Management System");
            Console.WriteLine("1. Item Management");
            Console.WriteLine("2. Stock Management");
            Console.WriteLine("3. Shelf Management");
            Console.WriteLine("4. Report Generation");
            Console.WriteLine("5. Billing System");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ItemManagement();
                    break;
                case "2":
                    StockManagement();
                    break;
                case "3":
                    ShelfManagement();
                    break;
                case "4":
                    ReportGeneration();
                    break;
                case "5":
                    BillingSystem();
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void ItemManagement()
    {
        Console.WriteLine("\nItem Management");
        Console.WriteLine("1. Add Item");
        Console.WriteLine("2. Edit Item");
        Console.WriteLine("3. Delete Item");
        Console.WriteLine("4. View All Items");
        Console.WriteLine("5. View Items by Category");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                AddItem();
                break;
            case "2":
                EditItem();
                break;
            case "3":
                DeleteItem();
                break;
            case "4":
                ViewAllItems();
                break;
            case "5":
                ViewItemsByCategory();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private static void ViewAllItems()
    {
        var items = inventoryFacade.GetAllItems();

        Console.WriteLine("\nItems:");
        Console.WriteLine("{0,-15} {1,-30} {2,-10}", "Item Code", "Item Name", "Price");
        Console.WriteLine(new string('-', 55));

        foreach (var item in items)
            Console.WriteLine("{0,-15} {1,-30} {2,-10:C}", item.ItemCode, item.Name, item.Price);
    }


    private static void AddItem()
    {
        Console.Write("Enter item code: ");
        var itemCode = Console.ReadLine();
        Console.Write("Enter item name: ");
        var itemName = Console.ReadLine();
        Console.Write("Enter item price: ");
        var itemPrice = decimal.Parse(Console.ReadLine());

        var newItem = new ItemDTO
        {
            ItemCode = itemCode,
            Name = itemName,
            Price = itemPrice
        };

        Console.Write("Enter category name: ");
        var categoryName = Console.ReadLine();

        // Find or create category
        var category = FindOrCreateCategory(categoryName);

        // Add item to category
        category.Add(new ItemLeaf(itemCode, itemName));

        inventoryFacade.AddItem(newItem);
        Console.WriteLine("Item added successfully.");
    }

    private static CategoryComposite FindOrCreateCategory(string categoryName)
    {
        foreach (var component in rootCategory.GetComponents())
            if (component is CategoryComposite category && category.CategoryName == categoryName)
                return category;

        var newCategory = new CategoryComposite(categoryName);
        rootCategory.Add(newCategory);
        return newCategory;
    }

    private static void EditItem()
    {
        Console.Write("Enter item code to edit: ");
        var itemCode = Console.ReadLine();

        Console.Write("Enter new item name: ");
        var newItemName = Console.ReadLine();
        Console.Write("Enter new item price: ");
        var newItemPrice = decimal.Parse(Console.ReadLine());

        var updatedItem = new ItemDTO
        {
            ItemCode = itemCode,
            Name = newItemName,
            Price = newItemPrice
        };

        inventoryFacade.EditItem(updatedItem);
        Console.WriteLine("Item edited successfully.");
    }

    private static void DeleteItem()
    {
        Console.Write("Enter item code to delete: ");
        var itemCode = Console.ReadLine();

        inventoryFacade.DeleteItem(itemCode);
        Console.WriteLine("Item deleted successfully.");
    }

    private static void ViewItemsByCategory()
    {
        inventoryFacade.DisplayItemsUnderCategory(rootCategory);
    }

    private static void StockManagement()
    {
        Console.WriteLine("\nStock Management");
        Console.WriteLine("1. Add Stock");
        Console.WriteLine("2. Delete Stock");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                AddStock();
                break;
            case "2":
                DeleteStock();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private static void AddStock()
    {
        Console.Write("Enter item code: ");
        var itemCode = Console.ReadLine();
        Console.Write("Enter quantity: ");
        var quantity = int.Parse(Console.ReadLine());
        var dateOfPurchase = DateTime.Now;
        Console.Write("Enter expiry date (YYYY-MM-DD): ");
        var expiryDate = DateTime.Parse(Console.ReadLine());

        var newStock = new StockDTO
        {
            ItemCode = itemCode,
            Quantity = quantity,
            DateOfPurchase = dateOfPurchase,
            ExpiryDate = expiryDate
        };

        inventoryFacade.AddStock(newStock);
        Console.WriteLine("Stock added successfully.");
    }

    private static void DeleteStock()
    {
        Console.Write("Enter stock ID to delete: ");
        var stockID = int.Parse(Console.ReadLine());

        inventoryFacade.DeleteStock(stockID);
        Console.WriteLine("Stock deleted successfully.");
    }

    private static void ShelfManagement()
    {
        Console.WriteLine("\nShelf Management");
        Console.WriteLine("1. Add Shelf");
        Console.WriteLine("2. Delete Shelf");
        Console.WriteLine("3. Assign Items to Shelf");
        Console.WriteLine("4. Display Shelves");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                AddShelf();
                break;
            case "2":
                DeleteShelf();
                break;
            case "3":
                AssignItemsToShelf();
                break;
            case "4":
                DisplayShelves();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private static void DisplayShelves()
    {
        var shelves = inventoryFacade.GetAllShelves();

        Console.WriteLine("\nShelves:");
        Console.WriteLine("{0,-10} {1,-20} {2,-15} {3,-20}", "Shelf ID", "Shelf Location", "Item Code",
            "Existing Quantity");
        Console.WriteLine(new string('-', 65));

        foreach (var shelf in shelves)
            Console.WriteLine("{0,-10} {1,-20} {2,-15} {3,-20}", shelf.ShelfID, shelf.ShelfLocation, shelf.ItemCode,
                shelf.ShelfQuantity);
    }


    private static void AddShelf()
    {
        Console.Write("Enter shelf location: ");
        var shelfLocation = Console.ReadLine();
        Console.Write("Enter item Code: ");
        var itemID = Console.ReadLine();
        Console.Write("Enter shelf quantity: ");
        var shelfQuantity = int.Parse(Console.ReadLine());

        var newShelf = new ShelfDTO
        {
            ShelfLocation = shelfLocation,
            ItemCode = itemID,
            ShelfQuantity = shelfQuantity
        };

        inventoryFacade.AddShelf(newShelf);
        Console.WriteLine("Shelf added successfully.");
    }

    private static void DeleteShelf()
    {
        Console.Write("Enter shelf ID to delete: ");
        var shelfID = int.Parse(Console.ReadLine());

        inventoryFacade.DeleteShelf(shelfID);
        Console.WriteLine("Shelf deleted successfully.");
    }

    private static void AssignItemsToShelf()
    {
        Console.Write("Enter shelf ID: ");
        var shelfID = int.Parse(Console.ReadLine());
        Console.Write("Enter item code: ");
        var itemCode = Console.ReadLine();
        Console.Write("Enter quantity to assign: ");
        var quantity = int.Parse(Console.ReadLine());

        // Assign items to shelf logic
        inventoryFacade.AssignItemsToShelf(shelfID, itemCode, quantity);
        Console.WriteLine("Items assigned to shelf successfully.");
    }

    private static void ReportGeneration()
    {
        Console.WriteLine("\nReport Generation");
        Console.WriteLine("1. Daily Sales Report");
        Console.WriteLine("2. Reshelving Report");
        Console.WriteLine("3. Reorder Levels Report");
        Console.WriteLine("4. Stock Report");
        Console.WriteLine("5. Bill Report");
        Console.Write("Select an option: ");
        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                GenerateDailySalesReport();
                break;
            case "2":
                GenerateReshelvingReport();
                break;
            case "3":
                GenerateReorderLevelsReport();
                break;
            case "4":
                GenerateStockReport();
                break;
            case "5":
                GenerateBillReport();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private static void GenerateDailySalesReport()
    {
        Console.Write("Enter date (YYYY-MM-DD) for sales report: ");
        var date = DateTime.Parse(Console.ReadLine());

        Report report = new DailySalesReport(date);
        report.GenerateReport();
    }

    private static void GenerateReshelvingReport()
    {
        Report report = new ReshelvingReport();
        report.GenerateReport();
    }

    private static void GenerateReorderLevelsReport()
    {
        Report report = new ReorderReport();
        report.GenerateReport();
    }

    private static void GenerateStockReport()
    {
        Report report = new StockReport();
        report.GenerateReport();
    }

    private static void GenerateBillReport()
    {
        Report report = new BillReport();
        report.GenerateReport();
    }

    private static void BillingSystem()
    {
        var billingFacade = new BillingFacade();
        billingFacade.ProcessSale();
    }
}