using SYOSSytem.Builder;
using SYOSSytem.Template;

namespace SYOSSytem.Facade;

public class ReportFacade
{
    private readonly ReportBuilder reportBuilder;

    public ReportFacade()
    {
        reportBuilder = new ReportBuilder();
    }

    public void GenerateDailySalesReport(DateTime date)
    {
        var report = reportBuilder.SetReport(new DailySalesReport(date)).Build();
        report.GenerateReport();
    }

    public void GenerateReshelvingReport()
    {
        var report = reportBuilder.SetReport(new ReshelvingReport()).Build();
        report.GenerateReport();
    }

    public void GenerateReorderReport()
    {
        var report = reportBuilder.SetReport(new ReorderReport()).Build();
        report.GenerateReport();
    }

    public void GenerateStockReport()
    {
        var report = reportBuilder.SetReport(new StockReport()).Build();
        report.GenerateReport();
    }

    public void GenerateBillReport()
    {
        var report = reportBuilder.SetReport(new BillReport()).Build();
        report.GenerateReport();
    }
}