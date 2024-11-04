using SYOSSytem.DTO;

namespace SYOSSytem.Template;

public class StockReport : Report
{
    protected override List<object> GetReportData()
    {
        return reportGateway.GetStockReport().Cast<object>().ToList();
    }

    protected override void DisplayReport(List<object> data)
    {
        var stockData = data.Cast<StockReportDTO>().ToList();
        Console.WriteLine("Stock Report:");
        Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-10} {4,-15}", "Stock ID", "Item Code", "Item Name", "Quantity",
            "Expiry Date");
        Console.WriteLine(new string('-', 70));

        foreach (var item in stockData)
            Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-10} {4,-15}", item.StockID, item.ItemCode, item.ItemName,
                item.Quantity, item.ExpiryDate?.ToString("yyyy-MM-dd") ?? "N/A");
    }
}