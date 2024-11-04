namespace SYOSSytem.DTO;

public class BillItemDTO
{
    public int BillItemID { get; set; }
    public string BillID { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}