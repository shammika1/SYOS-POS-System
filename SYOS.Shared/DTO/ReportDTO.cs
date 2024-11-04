using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYOS.Shared.DTO
{
    internal class ReportDTO
    {
    }

    public class DailySaleReportDTO
    {
        public DateTime Date { get; set; }
        public List<DailySaleItemDTO> Items { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class DailySaleItemDTO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ReshelveReportDTO
    {
        public List<ReshelveItemDTO> Items { get; set; }
    }

    public class ReshelveItemDTO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int QuantityToReshelve { get; set; }
    }

    public class ReorderReportDTO
    {
        public List<ReorderItemDTO> Items { get; set; }
    }

    public class ReorderItemDTO
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int CurrentStock { get; set; }
    }

    public class StockReportDTO
    {
        public List<StockBatchDTO> Batches { get; set; }
    }

    public class StockBatchDTO
    {
        public int BatchId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class BillReportDTO
    {
        public List<BillDTO> Bills { get; set; }
    }
}
