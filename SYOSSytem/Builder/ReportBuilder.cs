using SYOSSytem.Template;

namespace SYOSSytem.Builder;

public class ReportBuilder
{
    private Report report;

    public ReportBuilder SetReport(Report report)
    {
        this.report = report;
        return this;
    }

    public Report Build()
    {
        return report;
    }
}