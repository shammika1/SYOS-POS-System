using SYOSSytem.DataGateway;

namespace SYOSSytem.Template;

public abstract class Report
{
    protected ReportGateway reportGateway;

    public Report()
    {
        reportGateway = new ReportGateway();
    }

    public virtual void GenerateReport()
    {
        var data = GetReportData();
        DisplayReport(data);
    }

    protected abstract List<object> GetReportData();
    protected abstract void DisplayReport(List<object> data);
}