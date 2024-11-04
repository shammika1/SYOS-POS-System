using SYOSSytem.Builder;
using SYOSSytem.Decorator;
using SYOSSytem.DTO;
using SYOSSytem.State;

namespace SYOSSytem.Command;

public class ProcessSaleCommand : ICommand
{
    private readonly BillingContext _context;

    public ProcessSaleCommand(BillingContext context)
    {
        _context = context;
    }

    public void Execute()
    {
        var totalPrice = _context.BillItems.Sum(item => item.TotalPrice);
        var discountedPrice = totalPrice - _context.Discount;
        var changeAmount = _context.CashTendered - discountedPrice;
        var billDate = DateTime.Now;

        var bill = new BillBuilder()
            .SetBillID(null) // BillID will be generated
            .SetDate(billDate)
            .SetTotalPrice(totalPrice)
            .SetDiscount(_context.Discount)
            .SetCashTendered(_context.CashTendered)
            .SetChangeAmount(changeAmount)
            .Build();

        // Apply discount decorator
        BillDTO discountedBill = new DiscountDecorator(bill, _context.Discount);

        _context.BillGateway.AddBill(discountedBill);

        foreach (var billItem in _context.BillItems)
        {
            billItem.BillID = discountedBill.BillID;
            _context.BillItemGateway.AddBillItem(billItem);
        }

        _context.InventoryFacade.UpdateShelfQuantitiesAfterSale(_context.BillItems);

        DisplayBill(_context.BillItems, discountedBill);

        Console.WriteLine("Bill generated and saved successfully.");
    }

    private void DisplayBill(List<BillItemDTO> billItems, BillDTO bill)
    {
        Console.WriteLine("\nBill Summary:");
        Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10}", "Item Name", "Quantity", "Price", "Total Price");

        foreach (var billItem in billItems)
            Console.WriteLine("{0,-20} {1,-10} {2,-10:C} {3,-10:C}", billItem.ItemName, billItem.Quantity,
                billItem.TotalPrice / billItem.Quantity, billItem.TotalPrice);

        Console.WriteLine(new string('-', 60));
        Console.WriteLine("{0,-20} {1,-10} {2,-10:C}", "Total Price", "", bill.TotalPrice + bill.Discount);
        Console.WriteLine("{0,-20} {1,-10} {2,-10:C}", "Discount", "", bill.Discount);
        Console.WriteLine("{0,-20} {1,-10} {2,-10:C}", "Total after Discount", "", bill.TotalPrice);
        Console.WriteLine("{0,-20} {1,-10} {2,-10:C}", "Cash Tendered", "", bill.CashTendered);
        Console.WriteLine("{0,-20} {1,-10} {2,-10:C}", "Change", "", bill.ChangeAmount);
    }
}