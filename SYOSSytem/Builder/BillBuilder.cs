using SYOSSytem.DTO;

namespace SYOSSytem.Builder;

public class BillBuilder
{
    private readonly BillDTO bill;

    public BillBuilder()
    {
        bill = new BillDTO();
    }

    public BillBuilder SetBillID(string billID)
    {
        bill.BillID = billID;
        return this;
    }

    public BillBuilder SetDate(DateTime date)
    {
        bill.Date = date;
        return this;
    }

    public BillBuilder SetTotalPrice(decimal totalPrice)
    {
        bill.TotalPrice = totalPrice;
        return this;
    }

    public BillBuilder SetDiscount(decimal discount)
    {
        bill.Discount = discount;
        return this;
    }

    public BillBuilder SetCashTendered(decimal cashTendered)
    {
        bill.CashTendered = cashTendered;
        return this;
    }

    public BillBuilder SetChangeAmount(decimal changeAmount)
    {
        bill.ChangeAmount = changeAmount;
        return this;
    }

    public BillDTO Build()
    {
        return bill;
    }
}