using SYOSSytem.DTO;

namespace SYOSSytem.Template;

public class BillReport : Report
{
    protected override List<object> GetReportData()
    {
        return reportGateway.GetBillReport().Cast<object>().ToList();
    }

    protected override void DisplayReport(List<object> data)
    {
        var billData = data.Cast<BillReportDTO>().ToList();
        Console.WriteLine("Bill Report:");
        Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-10} {4,-15} {5,-15}", "Bill ID", "Date", "Total Price",
            "Discount", "Cash Tendered", "Change Amount");
        Console.WriteLine(new string('-', 90));

        foreach (var item in billData)
            Console.WriteLine("{0,-15} {1,-20} {2,-15:C} {3,-10:C} {4,-15:C} {5,-15:C}", item.BillID, item.Date,
                item.TotalPrice, item.Discount, item.CashTendered, item.ChangeAmount);
    }
}