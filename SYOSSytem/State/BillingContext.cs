using SYOSSytem.DataGateway;
using SYOSSytem.DTO;
using SYOSSytem.Facade;

namespace SYOSSytem.State;

public class BillingContext
{
    private IBillingState _state;

    public BillingContext(InventoryFacade inventoryFacade, BillGateway billGateway, BillItemGateway billItemGateway)
    {
        InventoryFacade = inventoryFacade;
        BillGateway = billGateway;
        BillItemGateway = billItemGateway;
    }

    public InventoryFacade InventoryFacade { get; set; }
    public BillGateway BillGateway { get; set; }
    public BillItemGateway BillItemGateway { get; set; }

    public List<BillItemDTO> BillItems { get; set; }
    public decimal Discount { get; set; }
    public decimal CashTendered { get; set; }

    public void SetState(IBillingState state)
    {
        _state = state;
        _state.Handle(this);
    }

    public IBillingState CurrentState()
    {
        return _state;
    }
}