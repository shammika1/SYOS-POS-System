namespace SYOSSytem.DTO;

public class StockDTO
{
    public int StockID { get; set; }
    public string ItemCode { get; set; }
    public int Quantity { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public DateTime? ExpiryDate { get; set; }
}