using SYOSSytem.DTO;

namespace SYOSSytem.Template;

public class ReshelvingReport : Report
{
    protected override List<object> GetReportData()
    {
        return reportGateway.GetReshelvingReport().Cast<object>().ToList();
    }

    protected override void DisplayReport(List<object> data)
    {
        var reshelvingData = data.Cast<ReshelvingReportDTO>().ToList();
        Console.WriteLine("Reshelving Report:");
        Console.WriteLine("{0,-15} {1,-20} {2,-20}", "Item Code", "Item Name", "Quantity to Reshelve");
        Console.WriteLine(new string('-', 55));

        foreach (var item in reshelvingData)
            Console.WriteLine("{0,-15} {1,-20} {2,-20}", item.ItemCode, item.ItemName, item.Quantity);
    }
}