namespace SYOSSytem.DTO;

public class BillReportDTO
{
    public string BillID { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal CashTendered { get; set; }
    public decimal ChangeAmount { get; set; }
}