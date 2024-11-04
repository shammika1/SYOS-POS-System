namespace SYOSSytem.DTO;

public class BillDTO
{
    public string BillID { get; set; }
    public DateTime Date { get; set; }
    public virtual decimal TotalPrice { get; set; }
    public virtual decimal Discount { get; set; }
    public decimal CashTendered { get; set; }
    public decimal ChangeAmount { get; set; }
}