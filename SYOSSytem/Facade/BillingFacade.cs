using SYOSSytem.DataGateway;
using SYOSSytem.State;

namespace SYOSSytem.Facade;

public class BillingFacade
{
    private BillGateway billGateway;
    private BillItemGateway billItemGateway;
    private InventoryFacade inventoryFacade;


    public BillingFacade()
    {
        inventoryFacade = new InventoryFacade();
        billGateway = new BillGateway();
        billItemGateway = new BillItemGateway();
    }

    public void Configure(InventoryFacade inventoryFacade, BillGateway billGateway, BillItemGateway billItemGateway)
    {
        this.inventoryFacade = inventoryFacade;
        this.billGateway = billGateway;
        this.billItemGateway = billItemGateway;
    }

    public void ProcessSale()
    {
        var billingContext = new BillingContext(inventoryFacade, billGateway, billItemGateway);
        billingContext.SetState(new ItemEntryState());
    }
}