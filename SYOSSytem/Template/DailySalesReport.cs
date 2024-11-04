using SYOSSytem.DTO;

namespace SYOSSytem.Template;

public class DailySalesReport : Report
{
    private readonly DateTime date;

    public DailySalesReport(DateTime date)
    {
        this.date = date;
    }

    protected override List<object> GetReportData()
    {
        return reportGateway.GetDailySalesReport(date).Cast<object>().ToList();
    }

    protected override void DisplayReport(List<object> data)
    {
        var salesData = data.Cast<SalesReportDTO>().ToList();
        Console.WriteLine("Daily Sales Report:");
        Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15}", "Item Code", "Item Name", "Total Quantity",
            "Total Revenue");
        Console.WriteLine(new string('-', 70));

        foreach (var item in salesData)
            Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-15:C}", item.ItemCode, item.ItemName, item.TotalQuantity,
                item.TotalRevenue);
    }
}