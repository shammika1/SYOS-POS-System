using SYOSSytem.DTO;
using SYOSSytem.Factory;

namespace SYOSSytem.State;

public class ItemEntryState : IBillingState
{
    public void Handle(BillingContext context)
    {
        var billItems = new List<BillItemDTO>();

        while (true)
        {
            Console.Write("Enter item code (or '00' to finish): ");
            var itemCode = Console.ReadLine();

            if (itemCode == "00") break;

            Console.Write("Enter quantity: ");
            var quantity = int.Parse(Console.ReadLine());

            var item = context.InventoryFacade.GetItem(itemCode);

            if (item != null)
            {
                var billItem =
                    DTOFactory.CreateBillItemDTO(null, item.ItemCode, item.Name, quantity, item.Price * quantity);
                billItems.Add(billItem);
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        context.BillItems = billItems;
        context.SetState(new PaymentState());
    }
}