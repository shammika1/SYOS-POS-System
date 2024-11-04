namespace SYOSSytem.DTO;

public class StockReportDTO
{
    public int StockID { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public DateTime? ExpiryDate { get; set; }
}