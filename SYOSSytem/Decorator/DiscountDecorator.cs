using SYOSSytem.DTO;

namespace SYOSSytem.Decorator;

public class DiscountDecorator : BillDecorator
{
    public DiscountDecorator(BillDTO bill, decimal discount) : base(bill)
    {
        this.bill.Discount = discount;
    }

    public override decimal TotalPrice => base.TotalPrice - base.Discount;
}