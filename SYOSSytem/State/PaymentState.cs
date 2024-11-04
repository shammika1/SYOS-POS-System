namespace SYOSSytem.State;

public class PaymentState : IBillingState
{
    public void Handle(BillingContext context)
    {
        Console.WriteLine("\nPreview of Items:");
        Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10}", "Item Name", "Quantity", "Price", "Total Price");

        foreach (var billItem in context.BillItems)
            Console.WriteLine("{0,-20} {1,-10} {2,-10:C} {3,-10:C}", billItem.ItemName, billItem.Quantity,
                billItem.TotalPrice / billItem.Quantity, billItem.TotalPrice);

        var totalPrice = context.BillItems.Sum(item => item.TotalPrice);
        Console.WriteLine(new string('-', 60));
        Console.WriteLine("{0,-20} {1,-10} {2,-10:C}", "Total Price", "", totalPrice);

        Console.Write("Enter discount: ");
        var discount = decimal.Parse(Console.ReadLine());

        Console.Write("Enter cash tendered: ");
        var cashTendered = decimal.Parse(Console.ReadLine());

        context.Discount = discount;
        context.CashTendered = cashTendered;

        context.SetState(new ReceiptGenerationState());
    }
}