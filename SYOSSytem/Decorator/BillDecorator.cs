using SYOSSytem.DTO;

namespace SYOSSytem.Decorator;

public abstract class BillDecorator : BillDTO
{
    protected BillDTO bill;

    public BillDecorator(BillDTO bill)
    {
        this.bill = bill;
    }

    public override decimal TotalPrice
    {
        get => bill.TotalPrice;
        set => bill.TotalPrice = value;
    }

    public override decimal Discount
    {
        get => bill.Discount;
        set => bill.Discount = value;
    }
}