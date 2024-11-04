using SYOSSytem.DTO;

namespace SYOSSytem.Template;

public class ReorderReport : Report
{
    protected override List<object> GetReportData()
    {
        return reportGateway.GetReorderReport().Cast<object>().ToList();
    }

    protected override void DisplayReport(List<object> data)
    {
        var reorderData = data.Cast<ReorderReportDTO>().ToList();
        Console.WriteLine("Reorder Report:");
        Console.WriteLine("{0,-15} {1,-20} {2,-20}", "Item Code", "Item Name", "Remaining Quantity");
        Console.WriteLine(new string('-', 55));

        foreach (var item in reorderData)
            Console.WriteLine("{0,-15} {1,-20} {2,-20}", item.ItemCode, item.ItemName, item.RemainingQuantity);
    }
}